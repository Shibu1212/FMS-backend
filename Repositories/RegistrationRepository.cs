using FormManagementSystem.Data;
using FormManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManagementSystem.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly AppDbContext _context;

    public RegistrationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RegistrationRequest> CreateAsync(
        RegistrationRequest request)
    {
        _context.RegistrationRequests.Add(request);

        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<IEnumerable<RegistrationRequest>> GetAsync(
    RegistrationStatus? status)
    {
        var query = _context.RegistrationRequests
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<RegistrationRequest?> GetByIdAsync(int id)
    {
        return await _context.RegistrationRequests
            .FindAsync(id);
    }

    public async Task UpdateAsync(RegistrationRequest request)
    {
        _context.RegistrationRequests.Update(request);

        await _context.SaveChangesAsync();
    }
}