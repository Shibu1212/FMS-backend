using FormManagementSystem.Data;
using FormManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FormManagementSystem.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByUserIdAsync(int userId)
    {
        return await _context.UserProfiles
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Educations)
            .Include(p => p.Experiences)
            .Include(p => p.Certifications)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<List<Education>> GetEducationsByUserIdAsync(int userId)
    {
        return await _context.Educations
            .AsNoTracking()
            .Where(e => e.UserProfile.UserId == userId)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<Education?> GetEducationByIdAsync(int educationId, int userId)
    {
        return await _context.Educations
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == educationId && e.UserProfile.UserId == userId);
    }

    public async Task<Education> CreateEducationAsync(Education education)
    {
        _context.Educations.Add(education);
        await _context.SaveChangesAsync();
        return education;
    }

    public async Task<bool> UpdateEducationAsync(Education education)
    {
        _context.Educations.Update(education);
        _context.Entry(education).Property(e => e.UserProfileId).IsModified = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEducationAsync(Education education)
    {
        _context.Educations.Remove(education);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Experience>> GetExperiencesByUserIdAsync(int userId)
    {
        return await _context.Experiences
            .AsNoTracking()
            .Where(e => e.UserProfile.UserId == userId)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<Experience?> GetExperienceByIdAsync(int experienceId, int userId)
    {
        return await _context.Experiences
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == experienceId && e.UserProfile.UserId == userId);
    }

    public async Task<Experience> CreateExperienceAsync(Experience experience)
    {
        _context.Experiences.Add(experience);
        await _context.SaveChangesAsync();
        return experience;
    }

    public async Task<bool> UpdateExperienceAsync(Experience experience)
    {
        _context.Experiences.Update(experience);
        _context.Entry(experience).Property(e => e.UserProfileId).IsModified = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteExperienceAsync(Experience experience)
    {
        _context.Experiences.Remove(experience);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Certification>> GetCertificationsByUserIdAsync(int userId)
    {
        return await _context.Certifications
            .AsNoTracking()
            .Where(c => c.UserProfile.UserId == userId)
            .OrderByDescending(c => c.IssueDate)
            .ToListAsync();
    }

    public async Task<Certification?> GetCertificationByIdAsync(int certificationId, int userId)
    {
        return await _context.Certifications
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == certificationId && c.UserProfile.UserId == userId);
    }

    public async Task<Certification> CreateCertificationAsync(Certification certification)
    {
        _context.Certifications.Add(certification);
        await _context.SaveChangesAsync();
        return certification;
    }

    public async Task<bool> UpdateCertificationAsync(Certification certification)
    {
        _context.Certifications.Update(certification);
        _context.Entry(certification).Property(c => c.UserProfileId).IsModified = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCertificationAsync(Certification certification)
    {
        _context.Certifications.Remove(certification);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProfilePictureAsync(int userId, string? profilePictureUrl)
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null)
            return false;

        profile.ProfilePictureUrl = profilePictureUrl;
        profile.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProfileAsync(int userId, string name)
    {
        var profile = await _context.UserProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return false;

        if (profile.User != null)
        {
            profile.User.Name = name;
            profile.User.UpdatedAt = DateTime.UtcNow;
        }

        profile.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
