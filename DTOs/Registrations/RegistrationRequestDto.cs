using System.ComponentModel.DataAnnotations;

namespace FormManagementSystem.DTOs.Registration;

public class RegistrationRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
}