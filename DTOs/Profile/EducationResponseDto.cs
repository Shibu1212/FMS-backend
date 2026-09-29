namespace FormManagementSystem.DTOs.Profile;

public class EducationResponseDto
{
    public int Id { get; set; }

    public string EducationType { get; set; } = string.Empty;

    public string Degree { get; set; } = string.Empty;

    public string FieldOfStudy { get; set; } = string.Empty;

    public string InstitutionName { get; set; } = string.Empty;

    public string InstitutionLocation { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrentlyStudying { get; set; }

    public string GradeType { get; set; } = string.Empty;

    public string? Grade { get; set; }

    public decimal? Percentage { get; set; }

    public decimal? CGPA { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
