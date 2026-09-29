namespace FormManagementSystem.DTOs.Profile;

public class ProfileResponseDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? ProfilePictureUrl { get; set; }

    public List<EducationResponseDto> Educations { get; set; } = new();

    public List<ExperienceResponseDto> Experiences { get; set; } = new();

    public List<CertificationResponseDto> Certifications { get; set; } = new();
}
