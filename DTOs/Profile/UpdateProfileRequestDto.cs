using System.ComponentModel.DataAnnotations;

namespace FormManagementSystem.DTOs.Profile;

public class UpdateProfileRequestDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
