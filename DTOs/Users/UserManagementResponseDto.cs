using FormManagementSystem.DTOs.Profile;

namespace FormManagementSystem.DTOs.Users;

public class UserManagementResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public string? Role { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public List<EducationResponseDto> Educations { get; set; } = new();

    public List<ExperienceResponseDto> Experiences { get; set; } = new();

    public List<CertificationResponseDto> Certifications { get; set; } = new();
}