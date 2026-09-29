namespace FormManagementSystem.DTOs.Forms;

public class UpdateFormFieldRequestDto
{
    public string Label { get; set; } = string.Empty;

    public string FieldType { get; set; } = string.Empty;

    public bool IsRequired { get; set; }

    public int DisplayOrder { get; set; }

    public string? Options { get; set; }
}