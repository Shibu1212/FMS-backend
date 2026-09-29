using FormManagementSystem.DTOs.Profile;
using FormManagementSystem.Models;
using FormManagementSystem.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FormManagementSystem.Services;

public class ProfileService : IProfileService
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private readonly IProfileRepository _profileRepository;
    private readonly IWebHostEnvironment _environment;

    public ProfileService(IProfileRepository profileRepository, IWebHostEnvironment environment)
    {
        _profileRepository = profileRepository;
        _environment = environment;
    }

    public async Task<ProfileResponseDto?> GetMyProfileAsync(int userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
            return null;

        return new ProfileResponseDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Name = profile.User?.Name ?? string.Empty,
            Email = profile.User?.Email ?? string.Empty,
            ProfilePictureUrl = profile.ProfilePictureUrl,
            Educations = profile.Educations.Select(MapToEducationResponseDto).ToList(),
            Experiences = profile.Experiences.Select(MapToExperienceResponseDto).ToList(),
            Certifications = profile.Certifications.Select(MapToCertificationResponseDto).ToList()
        };
    }

    public async Task<ProfileResponseDto?> UpdateProfileAsync(int userId, UpdateProfileRequestDto request)
    {
        var updated = await _profileRepository.UpdateProfileAsync(userId, request.Name);
        if (!updated)
            return null;

        return await GetMyProfileAsync(userId);
    }

    public async Task<List<EducationResponseDto>> GetMyEducationsAsync(int userId)
    {
        var educations = await _profileRepository.GetEducationsByUserIdAsync(userId);
        return educations.Select(MapToEducationResponseDto).ToList();
    }

    public async Task<EducationResponseDto?> GetMyEducationByIdAsync(int userId, int educationId)
    {
        var education = await _profileRepository.GetEducationByIdAsync(educationId, userId);
        if (education == null)
            return null;

        return MapToEducationResponseDto(education);
    }

    public async Task<EducationResponseDto> CreateEducationAsync(int userId, CreateEducationRequestDto request)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        var education = new Education
        {
            UserProfileId = profile.Id,
            EducationType = request.EducationType,
            Degree = request.Degree,
            FieldOfStudy = request.FieldOfStudy,
            InstitutionName = request.InstitutionName,
            InstitutionLocation = request.InstitutionLocation ?? string.Empty,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrentlyStudying = request.IsCurrentlyStudying,
            GradeType = request.GradeType,
            Grade = request.Grade,
            Percentage = request.Percentage,
            CGPA = request.CGPA,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _profileRepository.CreateEducationAsync(education);
        return MapToEducationResponseDto(created);
    }

    public async Task<EducationResponseDto?> UpdateEducationAsync(int userId, int educationId, UpdateEducationRequestDto request)
    {
        var education = await _profileRepository.GetEducationByIdAsync(educationId, userId);
        if (education == null)
            return null;

        education.EducationType = request.EducationType;
        education.Degree = request.Degree;
        education.FieldOfStudy = request.FieldOfStudy;
        education.InstitutionName = request.InstitutionName;
        education.InstitutionLocation = request.InstitutionLocation ?? string.Empty;
        education.StartDate = request.StartDate;
        education.EndDate = request.EndDate;
        education.IsCurrentlyStudying = request.IsCurrentlyStudying;
        education.GradeType = request.GradeType;
        education.Grade = request.Grade;
        education.Percentage = request.Percentage;
        education.CGPA = request.CGPA;
        education.Description = request.Description;
        education.UpdatedAt = DateTime.UtcNow;

        await _profileRepository.UpdateEducationAsync(education);
        return MapToEducationResponseDto(education);
    }

    public async Task<bool> DeleteEducationAsync(int userId, int educationId)
    {
        var education = await _profileRepository.GetEducationByIdAsync(educationId, userId);
        if (education == null)
            return false;

        return await _profileRepository.DeleteEducationAsync(education);
    }

    public async Task<List<ExperienceResponseDto>> GetMyExperiencesAsync(int userId)
    {
        var experiences = await _profileRepository.GetExperiencesByUserIdAsync(userId);
        return experiences.Select(MapToExperienceResponseDto).ToList();
    }

    public async Task<ExperienceResponseDto?> GetMyExperienceByIdAsync(int userId, int experienceId)
    {
        var experience = await _profileRepository.GetExperienceByIdAsync(experienceId, userId);
        if (experience == null)
            return null;

        return MapToExperienceResponseDto(experience);
    }

    public async Task<ExperienceResponseDto> CreateExperienceAsync(int userId, CreateExperienceRequestDto request)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        var experience = new Experience
        {
            UserProfileId = profile.Id,
            CompanyName = request.CompanyName,
            JobTitle = request.JobTitle,
            EmploymentType = request.EmploymentType,
            Location = request.Location ?? string.Empty,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrent = request.IsCurrent,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _profileRepository.CreateExperienceAsync(experience);
        return MapToExperienceResponseDto(created);
    }

    public async Task<ExperienceResponseDto?> UpdateExperienceAsync(int userId, int experienceId, UpdateExperienceRequestDto request)
    {
        var experience = await _profileRepository.GetExperienceByIdAsync(experienceId, userId);
        if (experience == null)
            return null;

        experience.CompanyName = request.CompanyName;
        experience.JobTitle = request.JobTitle;
        experience.EmploymentType = request.EmploymentType;
        experience.Location = request.Location ?? string.Empty;
        experience.StartDate = request.StartDate;
        experience.EndDate = request.EndDate;
        experience.IsCurrent = request.IsCurrent;
        experience.Description = request.Description;
        experience.UpdatedAt = DateTime.UtcNow;

        await _profileRepository.UpdateExperienceAsync(experience);
        return MapToExperienceResponseDto(experience);
    }

    public async Task<bool> DeleteExperienceAsync(int userId, int experienceId)
    {
        var experience = await _profileRepository.GetExperienceByIdAsync(experienceId, userId);
        if (experience == null)
            return false;

        return await _profileRepository.DeleteExperienceAsync(experience);
    }

    public async Task<List<CertificationResponseDto>> GetMyCertificationsAsync(int userId)
    {
        var certifications = await _profileRepository.GetCertificationsByUserIdAsync(userId);
        return certifications.Select(MapToCertificationResponseDto).ToList();
    }

    public async Task<CertificationResponseDto?> GetMyCertificationByIdAsync(int userId, int certificationId)
    {
        var certification = await _profileRepository.GetCertificationByIdAsync(certificationId, userId);
        if (certification == null)
            return null;

        return MapToCertificationResponseDto(certification);
    }

    public async Task<CertificationResponseDto> CreateCertificationAsync(int userId, CreateCertificationRequestDto request)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        var certification = new Certification
        {
            UserProfileId = profile.Id,
            Name = request.Name,
            IssuingOrganization = request.IssuingOrganization,
            CredentialId = request.CredentialId,
            CredentialUrl = request.CredentialUrl,
            IssueDate = request.IssueDate,
            ExpirationDate = request.ExpirationDate,
            DoesNotExpire = request.DoesNotExpire,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _profileRepository.CreateCertificationAsync(certification);
        return MapToCertificationResponseDto(created);
    }

    public async Task<CertificationResponseDto?> UpdateCertificationAsync(int userId, int certificationId, UpdateCertificationRequestDto request)
    {
        var certification = await _profileRepository.GetCertificationByIdAsync(certificationId, userId);
        if (certification == null)
            return null;

        certification.Name = request.Name;
        certification.IssuingOrganization = request.IssuingOrganization;
        certification.CredentialId = request.CredentialId;
        certification.CredentialUrl = request.CredentialUrl;
        certification.IssueDate = request.IssueDate;
        certification.ExpirationDate = request.ExpirationDate;
        certification.DoesNotExpire = request.DoesNotExpire;
        certification.Description = request.Description;
        certification.UpdatedAt = DateTime.UtcNow;

        await _profileRepository.UpdateCertificationAsync(certification);
        return MapToCertificationResponseDto(certification);
    }

    public async Task<bool> DeleteCertificationAsync(int userId, int certificationId)
    {
        var certification = await _profileRepository.GetCertificationByIdAsync(certificationId, userId);
        if (certification == null)
            return false;

        return await _profileRepository.DeleteCertificationAsync(certification);
    }

    private static EducationResponseDto MapToEducationResponseDto(Education e)
    {
        return new EducationResponseDto
        {
            Id = e.Id,
            EducationType = e.EducationType.ToString(),
            Degree = e.Degree,
            FieldOfStudy = e.FieldOfStudy,
            InstitutionName = e.InstitutionName,
            InstitutionLocation = e.InstitutionLocation,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            IsCurrentlyStudying = e.IsCurrentlyStudying,
            GradeType = e.GradeType.ToString(),
            Grade = e.Grade,
            Percentage = e.Percentage,
            CGPA = e.CGPA,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };
    }

    private static ExperienceResponseDto MapToExperienceResponseDto(Experience ex)
    {
        return new ExperienceResponseDto
        {
            Id = ex.Id,
            CompanyName = ex.CompanyName,
            JobTitle = ex.JobTitle,
            EmploymentType = ex.EmploymentType.ToString(),
            Location = ex.Location,
            StartDate = ex.StartDate,
            EndDate = ex.EndDate,
            IsCurrent = ex.IsCurrent,
            Description = ex.Description,
            CreatedAt = ex.CreatedAt,
            UpdatedAt = ex.UpdatedAt
        };
    }

    private static CertificationResponseDto MapToCertificationResponseDto(Certification c)
    {
        return new CertificationResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            IssuingOrganization = c.IssuingOrganization,
            CredentialId = c.CredentialId,
            CredentialUrl = c.CredentialUrl,
            IssueDate = c.IssueDate,
            ExpirationDate = c.ExpirationDate,
            DoesNotExpire = c.DoesNotExpire,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };
    }

    public async Task<ProfilePictureResponseDto> UploadProfilePictureAsync(int userId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Please select an image file to upload.");
        }

        if (file.Length > MaxFileSize)
        {
            throw new ArgumentException("File size cannot exceed 5 MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
        {
            throw new ArgumentException("Only JPG, JPEG, PNG, and WEBP files are allowed.");
        }

        var contentType = file.ContentType.ToLowerInvariant();
        if (string.IsNullOrEmpty(contentType) || !AllowedContentTypes.Contains(contentType))
        {
            throw new ArgumentException("Invalid content type. Only JPG, JPEG, PNG, and WEBP are allowed.");
        }

        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        var webRoot = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
        {
            webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var uploadsFolder = Path.Combine(webRoot, "uploads", "profile-pictures");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativeUrl = $"/uploads/profile-pictures/{uniqueFileName}";
        var previousPictureUrl = profile.ProfilePictureUrl;

        await _profileRepository.UpdateProfilePictureAsync(userId, relativeUrl);

        if (!string.IsNullOrWhiteSpace(previousPictureUrl))
        {
            DeleteLocalFile(previousPictureUrl);
        }

        return new ProfilePictureResponseDto
        {
            ProfilePictureUrl = relativeUrl
        };
    }

    public async Task<bool> DeleteProfilePictureAsync(int userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        if (profile == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(profile.ProfilePictureUrl))
        {
            DeleteLocalFile(profile.ProfilePictureUrl);
            await _profileRepository.UpdateProfilePictureAsync(userId, null);
        }

        return true;
    }

    private void DeleteLocalFile(string? relativeUrl)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl))
            return;

        try
        {
            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
            }

            var uploadsFolder = Path.Combine(webRoot, "uploads", "profile-pictures");
            var fileName = Path.GetFileName(relativeUrl);
            var filePath = Path.Combine(uploadsFolder, fileName);

            var fullPath = Path.GetFullPath(filePath);
            var fullUploadsFolder = Path.GetFullPath(uploadsFolder);

            if (fullPath.StartsWith(fullUploadsFolder, StringComparison.OrdinalIgnoreCase) && File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch
        {
            // Ignore file deletion errors to keep API idempotent and safe
        }
    }
}
