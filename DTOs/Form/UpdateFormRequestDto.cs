namespace FormManagementSystem.DTOs.Forms;

public class UpdateFormRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Status { get; set; } = string.Empty;
}