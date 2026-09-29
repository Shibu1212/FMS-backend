using FormManagementSystem.DTOs.Roles;
using FormManagementSystem.Repositories;

namespace FormManagementSystem.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(r => new RoleResponseDto
        {
            Id = r.Id,
            Name = r.Name
        });
    }

    public async Task<RoleResponseDto?> GetByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            return null;

        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }

    public async Task<RoleResponseDto?> GetByNameAsync(string name)
    {
        var role = await _roleRepository.GetByNameAsync(name);
        if (role == null)
            return null;

        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}
