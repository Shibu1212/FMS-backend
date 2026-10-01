using FormManagementSystem.DTOs.Common;
using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IFormResponseRepository
{
    Task<FormResponse> CreateAsync(FormResponse response);

    Task<FormResponse?> GetByIdAsync(int id);

    Task<(IEnumerable<FormResponse> Items, int TotalCount)> GetByUserIdAsync(
    int userId,
    PaginationRequestDto request);

    Task<(IEnumerable<FormResponse> Items, int TotalCount)> GetByFormIdAsync(
        int formId,
        PaginationRequestDto request);
    Task<bool> ExistsByFormAndUserAsync(int formId, int userId);
    
}