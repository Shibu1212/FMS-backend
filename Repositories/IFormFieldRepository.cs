using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IFormFieldRepository
{
    Task<FormField> CreateAsync(FormField field);

    Task<IEnumerable<FormField>> GetByFormIdAsync(int formId);

    Task<FormField?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(FormField field);

    Task<bool> DeleteAsync(int id);
}