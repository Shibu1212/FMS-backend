using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IFormResponseRepository
{
    Task<FormResponse> CreateAsync(FormResponse response);

    Task<FormResponse?> GetByIdAsync(int id);

    Task<List<FormResponse>> GetByUserIdAsync(int userId);

    Task<List<FormResponse>> GetByFormIdAsync(
    int formId,
    string? search = null);
    Task<bool> ExistsByFormAndUserAsync(int formId, int userId);
    
}