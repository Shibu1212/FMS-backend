using FormManagementSystem.Configuration;
using FormManagementSystem.Data;
using FormManagementSystem.DTOs.Auth;
using FormManagementSystem.Helpers;
using FormManagementSystem.Models;
using FormManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FormManagementSystem.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHelper _passwordHelper;
    private readonly JwtHelper _jwtHelper;
    private readonly RefreshTokenHelper _refreshTokenHelper;
    private readonly AppDbContext _context;
    private readonly PasswordResetTokenHelper _passwordResetTokenHelper;
    private readonly IEmailService _emailService;
    private readonly EmailSettings _emailSettings;
    private readonly int _accessTokenExpirySeconds;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        PasswordHelper passwordHelper,
        JwtHelper jwtHelper,
        RefreshTokenHelper refreshTokenHelper,
        PasswordResetTokenHelper passwordResetTokenHelper,
        AppDbContext context,
        IEmailService emailService,
        IOptions<EmailSettings> emailSettings,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _jwtHelper = jwtHelper;
        _refreshTokenHelper = refreshTokenHelper;
        _passwordResetTokenHelper = passwordResetTokenHelper;
        _context = context;
        _emailService = emailService;
        _emailSettings = emailSettings.Value;
        var expiryMinutes = configuration.GetValue<int>("Jwt:AccessTokenExpiryMinutes");
        _accessTokenExpirySeconds = (expiryMinutes > 0 ? expiryMinutes : 60) * 60;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto dto)
    {
        var email = dto.Email
            .Trim()
            .ToLowerInvariant();

        var user = await _userRepository
            .GetByEmailAsync(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "User account is inactive.");
        }

        var passwordValid = _passwordHelper
            .VerifyPassword(
                user,
                user.PasswordHash,
                dto.Password);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var accessToken =
            _jwtHelper.GenerateAccessToken(user);

        var refreshToken =
    _refreshTokenHelper.Generate();

        var refreshTokenHash =
            _refreshTokenHelper.Hash(refreshToken);

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        await _context.SaveChangesAsync();

        var roleName = user.UserRoles?.FirstOrDefault()?.Role?.Name ?? string.Empty;

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _accessTokenExpirySeconds,
            MustChangePassword = user.MustChangePassword,
            Role = roleName,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = roleName,
            }
        };
    }
    public async Task<LoginResponseDto> RefreshAsync(
    RefreshTokenRequestDto dto)
    {
        var tokenHash =
            _refreshTokenHelper.Hash(dto.RefreshToken);

        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(rt =>
                rt.TokenHash == tokenHash);

        if (refreshToken == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid refresh token.");
        }

        if (refreshToken.RevokedAt != null)
        {
            throw new UnauthorizedAccessException(
                "Refresh token has been revoked.");
        }

        if (refreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException(
                "Refresh token has expired.");
        }

        var user = refreshToken.User;
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "User account is inactive.");
        }

        var accessToken =
            _jwtHelper.GenerateAccessToken(user);

        var refreshRoleName = user.UserRoles?.FirstOrDefault()?.Role?.Name ?? string.Empty;

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = dto.RefreshToken,
            ExpiresIn = _accessTokenExpirySeconds,
            MustChangePassword = user.MustChangePassword,
            Role = refreshRoleName,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = refreshRoleName,
            }
        };
    }

    public async Task<bool> LogoutAsync(
    RefreshTokenRequestDto dto)
    {
        var tokenHash =
            _refreshTokenHelper.Hash(dto.RefreshToken);

        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt =>
                rt.TokenHash == tokenHash);

        if (refreshToken == null)
            return false;

        if (refreshToken.RevokedAt != null)
            return true;

        refreshToken.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task ChangePasswordAsync(
    int userId,
    ChangePasswordRequestDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new UnauthorizedAccessException(
                "User not found.");
        }

        var currentPasswordValid =
            _passwordHelper.VerifyPassword(
                user,
                user.PasswordHash,
                dto.CurrentPassword);

        if (!currentPasswordValid)
        {
            throw new UnauthorizedAccessException(
                "Current password is incorrect.");
        }


        var newPasswordIsSame =
        _passwordHelper.VerifyPassword(
            user,
            user.PasswordHash,
            dto.NewPassword);

            if (newPasswordIsSame)
            {
                throw new ArgumentException(
                    "New password cannot be the same as your current password.");
            }

        user.PasswordHash =
            _passwordHelper.HashPassword(
                user,
                dto.NewPassword);

        user.MustChangePassword = false;
        user.UpdatedAt = DateTime.UtcNow;

        var refreshTokens = await _context.RefreshTokens
            .Where(rt =>
                rt.UserId == userId &&
                rt.RevokedAt == null)
            .ToListAsync();

        foreach (var token in refreshTokens)
        {
            token.RevokedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return;
        }

        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !user.IsActive)
        {
            return;
        }

        var plainToken = _passwordResetTokenHelper.GenerateToken();

        var tokenHash = _passwordResetTokenHelper.HashToken(plainToken);

        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            CreatedAt = DateTime.UtcNow
        };

        _context.PasswordResetTokens.Add(resetToken);

        await _context.SaveChangesAsync();

        try
        {
            var separator = _emailSettings.PasswordResetUrl.Contains('?') ? "&" : "?";
            var resetLink = $"{_emailSettings.PasswordResetUrl}{separator}token={Uri.EscapeDataString(plainToken)}";

            var subject = "Password Reset Request";
            var body = EmailTemplateHelper.GetPasswordResetEmail(
                user.Name,
                user.Email,
                resetLink);

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}.", user.Email);
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDto dto)
    {
        var tokenHash = _passwordResetTokenHelper.HashToken(dto.Token);

        var resetToken = await _context.PasswordResetTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

        if (resetToken == null)
            throw new ArgumentException("Invalid reset token.");

        if (resetToken.UsedAt != null)
            throw new InvalidOperationException("Reset token has already been used.");

        if (resetToken.ExpiresAt < DateTime.UtcNow)
            throw new InvalidOperationException("Reset token has expired.");

        var user = resetToken.User;

        user.PasswordHash = _passwordHelper.HashPassword(
            user,
            dto.NewPassword
        );

        user.MustChangePassword = false;
        user.UpdatedAt = DateTime.UtcNow;

        // Revoke existing refresh tokens
        var refreshTokens = await _context.RefreshTokens
            .Where(x => x.UserId == user.Id && x.RevokedAt == null)
            .ToListAsync();

        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;
        }

        // Mark reset token as used
        resetToken.UsedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}