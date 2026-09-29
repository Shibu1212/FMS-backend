namespace FormManagementSystem.Models;

public class UserProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Education> Educations { get; set; } = new List<Education>();

    public ICollection<Experience> Experiences { get; set; } = new List<Experience>();

    public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
}