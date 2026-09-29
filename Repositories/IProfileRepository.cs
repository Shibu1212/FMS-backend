using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(int userId);

    Task<List<Education>> GetEducationsByUserIdAsync(int userId);

    Task<Education?> GetEducationByIdAsync(int educationId, int userId);

    Task<Education> CreateEducationAsync(Education education);

    Task<bool> UpdateEducationAsync(Education education);

    Task<bool> DeleteEducationAsync(Education education);

    Task<List<Experience>> GetExperiencesByUserIdAsync(int userId);

    Task<Experience?> GetExperienceByIdAsync(int experienceId, int userId);

    Task<Experience> CreateExperienceAsync(Experience experience);

    Task<bool> UpdateExperienceAsync(Experience experience);

    Task<bool> DeleteExperienceAsync(Experience experience);

    Task<List<Certification>> GetCertificationsByUserIdAsync(int userId);

    Task<Certification?> GetCertificationByIdAsync(int certificationId, int userId);

    Task<Certification> CreateCertificationAsync(Certification certification);

    Task<bool> UpdateCertificationAsync(Certification certification);

    Task<bool> DeleteCertificationAsync(Certification certification);

    Task<bool> UpdateProfilePictureAsync(int userId, string? profilePictureUrl);

    Task<bool> UpdateProfileAsync(int userId, string name);
}
