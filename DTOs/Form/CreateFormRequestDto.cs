namespace FormManagementSystem.DTOs.Forms;

public class CreateFormRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}