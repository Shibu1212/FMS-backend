using FormManagementSystem.Configuration;
using FormManagementSystem.Data;
using FormManagementSystem.DTOs.Auth;
using FormManagementSystem.DTOs.Registration;
using FormManagementSystem.Helpers;
using FormManagementSystem.Models;
using FormManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FormManagementSystem.Services;

public class RegistrationService : IRegistrationService
{
    private readonly AppDbContext _context;
    private readonly IRegistrationRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly PasswordHelper _passwordHelper;
    private readonly TemporaryPasswordHelper _passwordGenerator;
    private readonly IEmailService _emailService;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(
        AppDbContext context,
        IRegistrationRepository repository,
        IUserRepository userRepository,
        PasswordHelper passwordHelper,
        TemporaryPasswordHelper passwordGenerator,
        IEmailService emailService,
        IOptions<EmailSettings> emailSettings,
        ILogger<RegistrationService> logger)
    {
        _context = context;
        _repository = repository;
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _passwordGenerator = passwordGenerator;
        _emailService = emailService;
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<RegistrationRequest> CreateAsync(
        RegistrationRequestDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var existingUser = await _userRepository
            .GetByEmailAsync(email);

        if (existingUser != null)
            throw new InvalidOperationException(
                "A user with this email already exists.");

        var request = new RegistrationRequest
        {
            Name = dto.Name.Trim(),
            Email = email,
            Status = RegistrationStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };

        return await _repository.CreateAsync(request);
    }

    public async Task<IEnumerable<RegistrationResponseDto>> GetAsync(
        RegistrationStatus? status)
    {
        var requests = await _repository.GetAsync(status);

        return requests.Select(MapToResponse);
    }

    public async Task<RegistrationResponseDto?> GetByIdAsync(int id)
    {
        var request = await _repository.GetByIdAsync(id);

        if (request == null)
            return null;

        return MapToResponse(request);
    }

    public async Task<bool> UpdateStatusAsync(
        int id,
        RegistrationStatusDto dto,
        int adminUserId)
    {
        var request = await _repository.GetByIdAsync(id);

        if (request == null)
            return false;

        if (!Enum.TryParse<RegistrationStatus>(
                dto.Status,
                true,
                out var newStatus))
        {
            throw new ArgumentException(
                "Invalid registration status.");
        }

        if (request.Status != RegistrationStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending registration requests can be reviewed.");
        }

        // =========================
        // REJECT
        // =========================

        if (newStatus == RegistrationStatus.Rejected)
        {
            request.Status = RegistrationStatus.Rejected;
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewedBy = adminUserId;

            await _repository.UpdateAsync(request);

            return true;
        }

        // =========================
        // APPROVE
        // =========================

        if (newStatus != RegistrationStatus.Approved)
        {
            throw new ArgumentException(
                "Registration can only be Approved or Rejected.");
        }

        var existingUser = await _userRepository
            .GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "A user with this email already exists.");
        }

        string temporaryPassword;

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Generate temporary password
            temporaryPassword =
                _passwordGenerator.Generate();

            // 2. Create User
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                IsActive = true,
                MustChangePassword = true,
                CreatedAt = DateTime.UtcNow
            };

            // 3. Hash password
            user.PasswordHash =
                _passwordHelper.HashPassword(
                    user,
                    temporaryPassword);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            // 4. Find FORM_VIEWER role
            var viewerRole = await _context.Roles
                .FirstOrDefaultAsync(r =>
                    r.Name == "FORM_VIEWER");

            if (viewerRole == null)
            {
                throw new InvalidOperationException(
                    "FORM_VIEWER role does not exist.");
            }

            // 5. Assign FORM_VIEWER
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = viewerRole.Id
            };

            _context.UserRoles.Add(userRole);

            // 6. Create UserProfile
            var profile = new UserProfile
            {
                UserId = user.Id,
                ProfilePictureUrl = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.UserProfiles.Add(profile);

            // 7. Update registration
            request.Status = RegistrationStatus.Approved;
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewedBy = adminUserId;

            _context.RegistrationRequests.Update(request);

            // 8. Save everything
            await _context.SaveChangesAsync();

            // 9. Commit transaction
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        try
        {
            var subject = "Your Registration Has Been Approved";
            var body = EmailTemplateHelper.GetRegistrationApprovalEmail(
                request.Name,
                request.Email,
                temporaryPassword,
                "FORM_VIEWER",
                _emailSettings.LoginUrl);

            await _emailService.SendEmailAsync(request.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send registration approval email to {Email} for registration request ID {RequestId}.",
                request.Email,
                request.Id);
        }

        return true;
    }

    private static RegistrationResponseDto MapToResponse(
        RegistrationRequest request)
    {
        return new RegistrationResponseDto
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            Status = request.Status.ToString(),
            RequestedAt = request.RequestedAt,
            ReviewedAt = request.ReviewedAt,
            ReviewedBy = request.ReviewedBy
        };
    }

}

    