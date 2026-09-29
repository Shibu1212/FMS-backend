namespace FormManagementSystem.DTOs.FormResponse;

public class CreateFormResponseRequestDto
{
    public int FormId { get; set; }

    public List<FormResponseValueDto> Values { get; set; } = new();
}