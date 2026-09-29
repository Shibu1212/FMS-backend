namespace FormManagementSystem.Models;

public enum FormStatus
{
    DRAFT,
    PUBLISHED,
    ARCHIVED
}

public class Form
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public FormStatus Status { get; set; } = FormStatus.DRAFT;

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public ICollection<FormField> Fields { get; set; }
        = new List<FormField>();
}