using FormManagementSystem.Configuration;
using FormManagementSystem.DTOs.Profile;
using FormManagementSystem.DTOs.Users;
using FormManagementSystem.Helpers;
using FormManagementSystem.Models;
using FormManagementSystem.Repositories;
using Microsoft.Extensions.Options;

namespace FormManagementSystem.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly PasswordHelper _passwordHelper;
    private readonly TemporaryPasswordHelper _temporaryPasswordHelper;
    private readonly IEmailService _emailService;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<UserService> _logger;
    

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository, 
        PasswordHelper passwordHelper,
         TemporaryPasswordHelper temporaryPasswordHelper,
        IEmailService emailService,
        IOptions<EmailSettings> emailSettings,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHelper = passwordHelper;
        _temporaryPasswordHelper = temporaryPasswordHelper;
        _emailService = emailService;
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<ChangeRoleResult> ChangeUserRoleAsync(int userId, int roleId)
    {
        if (roleId <= 0)
        {
            return ChangeRoleResult.InvalidRequest;
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return ChangeRoleResult.UserNotFound;
        }

        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null)
        {
            return ChangeRoleResult.RoleNotFound;
        }

        var updated = await _userRepository.UpdateRoleAsync(userId, roleId);
        if (!updated)
        {
            return ChangeRoleResult.UserNotFound;
        }

        return ChangeRoleResult.Success;
    }

    public async Task<IEnumerable<UserManagementResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserManagementResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            IsActive = u.IsActive,
            MustChangePassword = u.MustChangePassword,

            Role = u.UserRoles
                .Select(ur => ur.Role.Name)
                .FirstOrDefault(),

            ProfilePictureUrl = u.UserProfile?.ProfilePictureUrl,

            Educations = u.UserProfile?.Educations
                .Select(e => new EducationResponseDto
                {
                    Id = e.Id,
                    EducationType = e.EducationType.ToString(),
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    InstitutionName = e.InstitutionName,
                    InstitutionLocation = e.InstitutionLocation,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    IsCurrentlyStudying = e.IsCurrentlyStudying,
                    GradeType = e.GradeType.ToString(),
                    Grade = e.Grade,
                    Percentage = e.Percentage,
                    CGPA = e.CGPA,
                    Description = e.Description
                })
                .ToList() ?? new(),

            Experiences = u.UserProfile?.Experiences
                .Select(e => new ExperienceResponseDto
                {
                    Id = e.Id,
                    CompanyName = e.CompanyName,
                    JobTitle = e.JobTitle,
                    EmploymentType = e.EmploymentType.ToString(),
                    Location = e.Location,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    IsCurrent = e.IsCurrent,
                    Description = e.Description
                })
                .ToList() ?? new(),

            Certifications = u.UserProfile?.Certifications
                .Select(c => new CertificationResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IssuingOrganization = c.IssuingOrganization,
                    CredentialId = c.CredentialId,
                    CredentialUrl = c.CredentialUrl,
                    IssueDate = c.IssueDate,
                    ExpirationDate = c.ExpirationDate,
                    DoesNotExpire = c.DoesNotExpire,
                    Description = c.Description
                })
                .ToList() ?? new()
        });
    }

    public async Task<bool> CreateByAdminAsync(CreateUserRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("Email is required.");
        }

        if (dto.RoleId <= 0)
        {
            throw new ArgumentException("Valid role is required.");
        }

        var email = dto.Email.Trim().ToLowerInvariant();

        var existingUser = await _userRepository.GetByEmailAsync(email);

        if (existingUser != null)
        {
            throw new ArgumentException(
                "A user with this email already exists.");
        }

        var role = await _roleRepository.GetByIdAsync(dto.RoleId);

        if (role == null)
        {
            throw new ArgumentException("Invalid role.");
        }

        var temporaryPassword = _temporaryPasswordHelper.Generate();

        var user = new User
        {
            Name = dto.Name.Trim(),
            Email = email,
            IsActive = true,
            MustChangePassword = true
        };

        user.PasswordHash =
            _passwordHelper.HashPassword(user, temporaryPassword);

        await _userRepository.CreateWithRoleAsync(
            user,
            dto.RoleId);

        try
        {
            var subject = "Your Account Has Been Created";

            var body = EmailTemplateHelper.GetRegistrationApprovalEmail(
                user.Name,
                user.Email,
                temporaryPassword,
                role.Name,
                _emailSettings.LoginUrl);

            await _emailService.SendEmailAsync(
                user.Email,
                subject,
                body);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send account creation email to {Email} for user ID {UserId}.",
                user.Email,
                user.Id);
        }

        return true;
    }

    public async Task<bool> UpdateAsync(
    int userId,
    UpdateUserRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("Email is required.");
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var email = dto.Email.Trim().ToLowerInvariant();

        var existingUser = await _userRepository.GetByEmailAsync(email);

        if (existingUser != null && existingUser.Id != userId)
        {
            throw new ArgumentException(
                "A user with this email already exists.");
        }

        user.Name = dto.Name.Trim();
        user.Email = email;

        return await _userRepository.UpdateAsync(user);
    }

    public async Task<ChangeUserStatusResult> ChangeUserStatusAsync(
    int userId,
    bool isActive)
    {
        if (userId <= 0)
        {
            return ChangeUserStatusResult.InvalidRequest;
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return ChangeUserStatusResult.UserNotFound;
        }

        var updated = await _userRepository.UpdateStatusAsync(
            userId,
            isActive);

        if (!updated)
        {
            return ChangeUserStatusResult.UserNotFound;
        }

        return ChangeUserStatusResult.Success;
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Invalid user ID.");
        }

        return await _userRepository.DeleteAsync(userId);
    }

    public async Task<int?> GetUserIdByPublicIdAsync(Guid publicId)
    {
        var user = await _userRepository.GetByPublicIdAsync(publicId);

        return user?.Id;
    }
}
