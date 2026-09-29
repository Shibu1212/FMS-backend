using FormManagementSystem.Data;
using FormManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManagementSystem.Repositories;

public class FormFieldRepository : IFormFieldRepository
{
    private readonly AppDbContext _context;

    public FormFieldRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FormField> CreateAsync(FormField field)
    {
        _context.FormFields.Add(field);

        await _context.SaveChangesAsync();

        return field;
    }

    public async Task<IEnumerable<FormField>> GetByFormIdAsync(int formId)
    {
        return await _context.FormFields
            .Where(field => field.FormId == formId)
            .OrderBy(field => field.DisplayOrder)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<FormField?> GetByIdAsync(int id)
    {
        return await _context.FormFields
            .FirstOrDefaultAsync(field => field.Id == id);
    }

    public async Task<bool> UpdateAsync(FormField field)
    {
        var existingField = await _context.FormFields
            .FirstOrDefaultAsync(f => f.Id == field.Id);

        if (existingField == null)
            return false;

        existingField.Label = field.Label;
        existingField.FieldType = field.FieldType;
        existingField.IsRequired = field.IsRequired;
        existingField.DisplayOrder = field.DisplayOrder;
        existingField.Options = field.Options;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var field = await _context.FormFields
            .FirstOrDefaultAsync(f => f.Id == id);

        if (field == null)
            return false;

        _context.FormFields.Remove(field);

        await _context.SaveChangesAsync();

        return true;
    }
}