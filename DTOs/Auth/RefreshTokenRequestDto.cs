using System.ComponentModel.DataAnnotations;

namespace FormManagementSystem.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}