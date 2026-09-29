using FormManagementSystem.DTOs.Profile;
using Microsoft.AspNetCore.Http;

namespace FormManagementSystem.Services;

public interface IProfileService
{
    Task<ProfileResponseDto?> GetMyProfileAsync(int userId);

    Task<List<EducationResponseDto>> GetMyEducationsAsync(int userId);

    Task<EducationResponseDto?> GetMyEducationByIdAsync(int userId, int educationId);

    Task<EducationResponseDto> CreateEducationAsync(int userId, CreateEducationRequestDto request);

    Task<EducationResponseDto?> UpdateEducationAsync(int userId, int educationId, UpdateEducationRequestDto request);

    Task<bool> DeleteEducationAsync(int userId, int educationId);

    Task<List<ExperienceResponseDto>> GetMyExperiencesAsync(int userId);

    Task<ExperienceResponseDto?> GetMyExperienceByIdAsync(int userId, int experienceId);

    Task<ExperienceResponseDto> CreateExperienceAsync(int userId, CreateExperienceRequestDto request);

    Task<ExperienceResponseDto?> UpdateExperienceAsync(int userId, int experienceId, UpdateExperienceRequestDto request);

    Task<bool> DeleteExperienceAsync(int userId, int experienceId);

    Task<List<CertificationResponseDto>> GetMyCertificationsAsync(int userId);

    Task<CertificationResponseDto?> GetMyCertificationByIdAsync(int userId, int certificationId);

    Task<CertificationResponseDto> CreateCertificationAsync(int userId, CreateCertificationRequestDto request);

    Task<CertificationResponseDto?> UpdateCertificationAsync(int userId, int certificationId, UpdateCertificationRequestDto request);

    Task<bool> DeleteCertificationAsync(int userId, int certificationId);

    Task<ProfilePictureResponseDto> UploadProfilePictureAsync(int userId, IFormFile file);

    Task<bool> DeleteProfilePictureAsync(int userId);

    Task<ProfileResponseDto?> UpdateProfileAsync(int userId, UpdateProfileRequestDto request);
}
