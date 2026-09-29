namespace FormManagementSystem.Models;

public enum EmploymentType
{
    FullTime,
    PartTime,
    Contract,
    Internship,
    Freelance,
    SelfEmployed,
    Other
}

public class Experience
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public EmploymentType EmploymentType { get; set; }

    public string Location { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrent { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
}
