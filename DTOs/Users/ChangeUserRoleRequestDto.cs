using System.ComponentModel.DataAnnotations;

namespace FormManagementSystem.DTOs.Users;

public class ChangeUserRoleRequestDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive integer.")]
    public int RoleId { get; set; }
}
