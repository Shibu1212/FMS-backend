using FormManagementSystem.Data;
using FormManagementSystem.DTOs.Common;
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

    public async Task<(IEnumerable<FormResponse> Items, int TotalCount)> GetByUserIdAsync(
    int userId,
    PaginationRequestDto request)
    {
        var query = _context.FormResponses
            .AsNoTracking()
            .Include(response => response.Form)
            .Include(response => response.SubmittedByUser)
            .Include(response => response.Values)
                .ThenInclude(value => value.FormField)
            .Where(response => response.SubmittedBy == userId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(response =>
                response.Form.Name.Contains(search));
        }

        query = request.SortBy?.ToLower() switch
        {
            "formname" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(response => response.Form.Name)
                : query.OrderByDescending(response => response.Form.Name),

            "submittedat" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(response => response.SubmittedAt)
                : query.OrderByDescending(response => response.SubmittedAt),

            _ => query.OrderByDescending(response => response.SubmittedAt)
        };

        var totalCount = await query.CountAsync();

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(IEnumerable<FormResponse> Items, int TotalCount)> GetByFormIdAsync(
    int formId,
    PaginationRequestDto request)
    {
        var query = _context.FormResponses
            .AsNoTracking()
            .Include(response => response.Form)
            .Include(response => response.SubmittedByUser)
            .Include(response => response.Values)
                .ThenInclude(value => value.FormField)
            .Where(response => response.FormId == formId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(response =>
                response.SubmittedByUser.Name.Contains(search) ||
                response.SubmittedByUser.Email.Contains(search));
        }

        query = request.SortBy?.ToLower() switch
        {
            "username" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(response => response.SubmittedByUser.Name)
                : query.OrderByDescending(response => response.SubmittedByUser.Name),

            "email" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(response => response.SubmittedByUser.Email)
                : query.OrderByDescending(response => response.SubmittedByUser.Email),

            "submittedat" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(response => response.SubmittedAt)
                : query.OrderByDescending(response => response.SubmittedAt),

            _ => query.OrderByDescending(response => response.SubmittedAt)
        };

        var totalCount = await query.CountAsync();

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
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