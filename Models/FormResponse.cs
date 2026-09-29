namespace FormManagementSystem.Models;

public class FormResponse
{
    public int Id { get; set; }

    public int FormId { get; set; }

    public int SubmittedBy { get; set; }

    public DateTime SubmittedAt { get; set; }

    // Audit fields
    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    // Navigation properties
    public Form Form { get; set; } = null!;

    public User SubmittedByUser { get; set; } = null!;

    public ICollection<FormResponseValue> Values { get; set; }
        = new List<FormResponseValue>();
}