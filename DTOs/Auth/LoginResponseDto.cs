namespace FormManagementSystem.DTOs.Auth;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public bool MustChangePassword { get; set; }

    public string Role { get; set; } = string.Empty;

    public UserDto User { get; set; } = null!;
}