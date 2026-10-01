using FormManagementSystem.DTOs.Common;
using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IFormRepository
{
    Task<Form> CreateAsync(Form form);

    Task<(IEnumerable<Form> Items, int TotalCount)> GetAllAsync(
        PaginationRequestDto request);

    Task<(IEnumerable<Form> Items, int TotalCount)> GetPublishedAsync(
        PaginationRequestDto request);

    Task<Form?> GetByIdAsync(int id);

    Task<Form?> GetPublishedByIdAsync(int id);

    Task<bool> UpdateAsync(Form form);

    Task<bool> DeleteAsync(int id);
}