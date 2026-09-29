namespace FormManagementSystem.Models;

public enum FormFieldType
{
    TEXT,
    TEXTAREA,
    NUMBER,
    EMAIL,
    DATE,
    DROPDOWN,
    RADIO,
    CHECKBOX
}

public class FormField
{
    public int Id { get; set; }

    public int FormId { get; set; }

    public string Label { get; set; } = string.Empty;

    public FormFieldType FieldType { get; set; }

    public bool IsRequired { get; set; }

    public int DisplayOrder { get; set; }

    public string? Options { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public Form Form { get; set; } = null!;
}