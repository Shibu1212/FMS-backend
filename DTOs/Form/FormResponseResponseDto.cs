namespace FormManagementSystem.DTOs.FormResponse;

public class FormResponseResponseDto
{
    public int Id { get; set; }

    public int FormId { get; set; }

    public string? FormName { get; set; }


    public int SubmittedBy { get; set; }

    public string? SubmittedByName { get; set; }

    public string? SubmittedByEmail { get; set; }

    public DateTime SubmittedAt { get; set; }

    public List<FormResponseValueDto> Values { get; set; } = new();
}