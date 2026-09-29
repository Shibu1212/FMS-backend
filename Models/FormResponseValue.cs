namespace FormManagementSystem.Models;

public class FormResponseValue
{
    public int Id { get; set; }

    public int FormResponseId { get; set; }

    public int FormFieldId { get; set; }

    public string Value { get; set; } = string.Empty;

    // Audit fields
    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    // Navigation properties
    public FormResponse FormResponse { get; set; } = null!;

    public FormField FormField { get; set; } = null!;
}