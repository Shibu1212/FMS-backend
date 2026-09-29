using FormManagementSystem.Data;
using FormManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManagementSystem.Repositories;

public class FormResponseRepository : IFormResponseRepository
{
    private readonly AppDbContext _context;

    public FormResponseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FormResponse> CreateAsync(FormResponse response)
    {
        _context.FormResponses.Add(response);

        await _context.SaveChangesAsync();

        return response;
    }

    public async Task<FormResponse?> GetByIdAsync(int id)
    {
        return await _context.FormResponses
            .AsNoTracking()
            .Include(response => response.Form)
            .Include(response => response.SubmittedByUser)
            .Include(response => response.Values)
                .ThenInclude(value => value.FormField)
            .FirstOrDefaultAsync(response => response.Id == id);
    }

    public async Task<List<FormResponse>> GetByUserIdAsync(int userId)
    {
        return await _context.FormResponses
            .AsNoTracking()
            .Include(response => response.Form)
            .Include(response => response.SubmittedByUser)
            .Include(response => response.Values)
                .ThenInclude(value => value.FormField)
            .Where(response => response.SubmittedBy == userId)
            .OrderByDescending(response => response.SubmittedAt)
            .ToListAsync();
    }

    public async Task<List<FormResponse>> GetByFormIdAsync(
    int formId,
    string? search = null)
    {
        var query = _context.FormResponses
            .AsNoTracking()
            .Include(response => response.Form)
            .Include(response => response.SubmittedByUser)
            .Include(response => response.Values)
                .ThenInclude(value => value.FormField)
            .Where(response => response.FormId == formId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(response =>
                response.SubmittedByUser.Name.Contains(search) ||
                response.SubmittedByUser.Email.Contains(search));
        }

        return await query
            .OrderByDescending(response => response.SubmittedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByFormAndUserAsync(
    int formId,
    int userId)
    {
        return await _context.FormResponses
            .AnyAsync(response =>
                response.FormId == formId &&
                response.SubmittedBy == userId);
    }
}