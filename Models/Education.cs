namespace FormManagementSystem.Models;

public enum EducationType
{
    School,
    HigherSecondary,
    Diploma,
    Bachelor,
    Master,
    Doctorate,
    Other
}

public enum GradeType
{
    Percentage,
    CGPA,
    GPA,
    Grade,
    Other
}

public class Education
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public EducationType EducationType { get; set; }

    public string Degree { get; set; } = string.Empty;

    public string FieldOfStudy { get; set; } = string.Empty;

    public string InstitutionName { get; set; } = string.Empty;

    public string InstitutionLocation { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrentlyStudying { get; set; }

    public GradeType GradeType { get; set; }

    public string? Grade { get; set; }

    public decimal? Percentage { get; set; }

    public decimal? CGPA { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
}
