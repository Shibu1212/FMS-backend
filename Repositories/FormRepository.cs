using FormManagementSystem.Data;
using FormManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using FormManagementSystem.DTOs.Common;

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

    public async Task<(IEnumerable<Form> Items, int TotalCount)> GetAllAsync(
    PaginationRequestDto request)
    {
        var query = _context.Forms
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(f =>
                f.Name.Contains(search) ||
                (f.Description != null &&
                 f.Description.Contains(search)));
        }

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(f => f.Name)
                : query.OrderByDescending(f => f.Name),

            "status" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(f => f.Status)
                : query.OrderByDescending(f => f.Status),

            "createdat" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(f => f.CreatedAt)
                : query.OrderByDescending(f => f.CreatedAt),

            _ => query.OrderByDescending(f => f.CreatedAt)
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

    public async Task<(IEnumerable<Form> Items, int TotalCount)> GetPublishedAsync(
    PaginationRequestDto request)
    {
        var query = _context.Forms
            .Where(f => f.Status == FormStatus.PUBLISHED)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(f =>
                f.Name.Contains(search) ||
                (f.Description != null &&
                 f.Description.Contains(search)));
        }

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(f => f.Name)
                : query.OrderByDescending(f => f.Name),

            "createdat" => request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(f => f.CreatedAt)
                : query.OrderByDescending(f => f.CreatedAt),

            _ => query.OrderByDescending(f => f.CreatedAt)
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