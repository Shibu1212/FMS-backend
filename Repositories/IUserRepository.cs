using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByPublicIdAsync(Guid publicId);

    Task<User> CreateAsync(User user);

    Task<User> CreateWithRoleAsync(User user, int roleId);
    Task<bool> UpdateAsync(User user);

    Task<bool> UpdateStatusAsync(
    int userId,
    bool isActive);

    Task<bool> UpdateRoleAsync(int userId, int roleId);

    Task<bool> DeleteAsync(int userId);

    Task<IEnumerable<User>> GetAllAsync();

}