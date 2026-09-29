using FormManagementSystem.DTOs.Auth;

namespace FormManagementSystem.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(
        LoginRequestDto dto);
    Task<LoginResponseDto> RefreshAsync(
    RefreshTokenRequestDto dto);

    Task<bool> LogoutAsync(
        RefreshTokenRequestDto dto);
    Task ChangePasswordAsync(
    int userId,
    ChangePasswordRequestDto dto);

    Task ForgotPasswordAsync(ForgotPasswordRequestDto dto);
    Task ResetPasswordAsync(ResetPasswordRequestDto dto);
}