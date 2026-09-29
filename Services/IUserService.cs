namespace FormManagementSystem.Services;

using FormManagementSystem.DTOs.Users;

public enum ChangeRoleResult
{
    Success,
    UserNotFound,
    RoleNotFound,
    InvalidRequest
}

public enum ChangeUserStatusResult
{
    Success,
    UserNotFound,
    InvalidRequest
}

public interface IUserService
{
    Task<ChangeRoleResult> ChangeUserRoleAsync(int userId, int roleId);

    Task<IEnumerable<UserManagementResponseDto>> GetAllAsync();

    Task<bool> CreateByAdminAsync(CreateUserRequestDto dto);

    Task<bool> UpdateAsync(int userId, UpdateUserRequestDto dto);

    Task<ChangeUserStatusResult> ChangeUserStatusAsync(
        int userId,
        bool isActive);
    Task<bool> DeleteAsync(int userId);

    Task<int?> GetUserIdByPublicIdAsync(Guid publicId);
}
