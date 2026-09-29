namespace FormManagementSystem.DTOs.Forms;

public class FormFieldResponseDto
{
    public int Id { get; set; }

    public int FormId { get; set; }

    public string Label { get; set; } = string.Empty;

    public string FieldType { get; set; } = string.Empty;

    public bool IsRequired { get; set; }

    public int DisplayOrder { get; set; }

    public string? Options { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public string? CreatedByName { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? UpdatedByName { get; set; }
}