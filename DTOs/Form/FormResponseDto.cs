namespace FormManagementSystem.DTOs.Forms;

public class FormResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public string? CreatedByName { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? UpdatedByName { get; set; }

    public List<FormFieldResponseDto> Fields { get; set; }
    = new List<FormFieldResponseDto>();
}