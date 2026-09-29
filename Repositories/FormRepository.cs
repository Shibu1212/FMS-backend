using FormManagementSystem.Data;
using FormManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManagementSystem.Repositories;

public class FormRepository : IFormRepository
{
    private readonly AppDbContext _context;

    public FormRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Form> CreateAsync(Form form)
    {
        _context.Forms.Add(form);

        await _context.SaveChangesAsync();

        return form;
    }

    public async Task<IEnumerable<Form>> GetAllAsync()
    {
        return await _context.Forms
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Form>> GetPublishedAsync()
    {
        return await _context.Forms
            .Where(f => f.Status == Models.FormStatus.PUBLISHED)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Form?> GetByIdAsync(int id)
    {
        return await _context.Forms
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Form?> GetPublishedByIdAsync(int id)
    {
        return await _context.Forms
            .FirstOrDefaultAsync(f =>
                f.Id == id &&
                f.Status == Models.FormStatus.PUBLISHED);
    }

    public async Task<bool> UpdateAsync(Form form)
    {
        var existingForm = await _context.Forms
            .FirstOrDefaultAsync(f => f.Id == form.Id);

        if (existingForm == null)
            return false;

        existingForm.Name = form.Name;
        existingForm.Description = form.Description;
        existingForm.Status = form.Status;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var form = await _context.Forms
            .FirstOrDefaultAsync(f => f.Id == id);

        if (form == null)
            return false;

        _context.Forms.Remove(form);

        await _context.SaveChangesAsync();

        return true;
    }
}