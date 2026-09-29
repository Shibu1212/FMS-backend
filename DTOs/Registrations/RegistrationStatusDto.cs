using System.ComponentModel.DataAnnotations;

namespace FormManagementSystem.DTOs.Registration;

public class RegistrationStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}