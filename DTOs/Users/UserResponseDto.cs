using FormManagementSystem.DTOs.Profile;

namespace FormManagementSystem.DTOs.Users;

public class UserResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public string? Role { get; set; }

    public ProfileResponseDto? Profile { get; set; }
}