using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IFormRepository
{
    Task<Form> CreateAsync(Form form);

    Task<IEnumerable<Form>> GetAllAsync();

    Task<IEnumerable<Form>> GetPublishedAsync();

    Task<Form?> GetByIdAsync(int id);

    Task<Form?> GetPublishedByIdAsync(int id);

    Task<bool> UpdateAsync(Form form);

    Task<bool> DeleteAsync(int id);
}