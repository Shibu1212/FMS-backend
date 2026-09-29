using FormManagementSystem.DTOs.Roles;

namespace FormManagementSystem.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleResponseDto>> GetAllAsync();
    Task<RoleResponseDto?> GetByIdAsync(int id);
    Task<RoleResponseDto?> GetByNameAsync(string name);
}
