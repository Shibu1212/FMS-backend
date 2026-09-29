using FormManagementSystem.DTOs.Profile;
using FormManagementSystem.Extensions;
using FormManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FormManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IUserService _userService;

    public ProfileController(
    IProfileService profileService,
    IUserService userService)
    {
        _profileService = profileService;
        _userService = userService;
    }

    private async Task<int?> GetCurrentUserIdAsync()
    {
        var publicId = User.GetPublicId();

        if (publicId is not { } currentPublicId)
        {
            return null;
        }

        return await _userService.GetUserIdByPublicIdAsync(
            currentPublicId);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var profile = await _profileService.GetMyProfileAsync(
            currentUserId);

        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var updated = await _profileService.UpdateProfileAsync(currentUserId, request);

        if (updated == null)
        {
            return NotFound(new
            {
                message = "Profile not found."
            });
        }

        return Ok(updated);
    }

    [HttpGet("educations")]
    public async Task<IActionResult> GetMyEducations()
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var educations = await _profileService.GetMyEducationsAsync(currentUserId);
        return Ok(educations);
    }

    [HttpGet("educations/{educationId:int}")]
    public async Task<IActionResult> GetMyEducationById(int educationId)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var education = await _profileService.GetMyEducationByIdAsync(currentUserId, educationId);

        if (education == null)
        {
            return NotFound(new
            {
                message = "Education record not found."
            });
        }

        return Ok(education);
    }

    [HttpPost("educations")]
    public async Task<IActionResult> CreateEducation([FromBody] CreateEducationRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var created = await _profileService.CreateEducationAsync(currentUserId, request);

        return CreatedAtAction(
            nameof(GetMyEducationById),
            new { educationId = created.Id },
            created);
    }

    [HttpPut("educations/{educationId:int}")]
    public async Task<IActionResult> UpdateEducation(int educationId, [FromBody] UpdateEducationRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var updated = await _profileService.UpdateEducationAsync(currentUserId, educationId, request);

        if (updated == null)
        {
            return NotFound(new
            {
                message = "Education record not found."
            });
        }

        return Ok(updated);
    }

    [HttpDelete("educations/{educationId:int}")]
    public async Task<IActionResult> DeleteEducation(int educationId)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var deleted = await _profileService.DeleteEducationAsync(currentUserId, educationId);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Education record not found."
            });
        }

        return NoContent();
    }

    [HttpGet("experiences")]
    public async Task<IActionResult> GetMyExperiences()
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var experiences = await _profileService.GetMyExperiencesAsync(currentUserId);
        return Ok(experiences);
    }

    [HttpGet("experiences/{experienceId:int}")]
    public async Task<IActionResult> GetMyExperienceById(int experienceId)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var experience = await _profileService.GetMyExperienceByIdAsync(currentUserId, experienceId);

        if (experience == null)
        {
            return NotFound(new
            {
                message = "Experience record not found."
            });
        }

        return Ok(experience);
    }

    [HttpPost("experiences")]
    public async Task<IActionResult> CreateExperience([FromBody] CreateExperienceRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var created = await _profileService.CreateExperienceAsync(currentUserId, request);

        return CreatedAtAction(
            nameof(GetMyExperienceById),
            new { experienceId = created.Id },
            created);
    }

    [HttpPut("experiences/{experienceId:int}")]
    public async Task<IActionResult> UpdateExperience(int experienceId, [FromBody] UpdateExperienceRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var updated = await _profileService.UpdateExperienceAsync(currentUserId, experienceId, request);

        if (updated == null)
        {
            return NotFound(new
            {
                message = "Experience record not found."
            });
        }

        return Ok(updated);
    }

    [HttpDelete("experiences/{experienceId:int}")]
    public async Task<IActionResult> DeleteExperience(int experienceId)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var deleted = await _profileService.DeleteExperienceAsync(currentUserId, experienceId);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Experience record not found."
            });
        }

        return NoContent();
    }

    [HttpGet("certifications")]
    public async Task<IActionResult> GetMyCertifications()
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var certifications = await _profileService.GetMyCertificationsAsync(currentUserId);
        return Ok(certifications);
    }

    [HttpGet("certifications/{certificationId:int}")]
    public async Task<IActionResult> GetMyCertificationById(int certificationId)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var certification = await _profileService.GetMyCertificationByIdAsync(currentUserId, certificationId);

        if (certification == null)
        {
            return NotFound(new
            {
                message = "Certification record not found."
            });
        }

        return Ok(certification);
    }

    [HttpPost("certifications")]
    public async Task<IActionResult> CreateCertification([FromBody] CreateCertificationRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var created = await _profileService.CreateCertificationAsync(currentUserId, request);

        return CreatedAtAction(
            nameof(GetMyCertificationById),
            new { certificationId = created.Id },
            created);
    }

    [HttpPut("certifications/{certificationId:int}")]
    public async Task<IActionResult> UpdateCertification(int certificationId, [FromBody] UpdateCertificationRequestDto request)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var updated = await _profileService.UpdateCertificationAsync(currentUserId, certificationId, request);

        if (updated == null)
        {
            return NotFound(new
            {
                message = "Certification record not found."
            });
        }

        return Ok(updated);
    }

    [HttpDelete("certifications/{certificationId:int}")]
    public async Task<IActionResult> DeleteCertification(int certificationId)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var deleted = await _profileService.DeleteCertificationAsync(currentUserId, certificationId);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Certification record not found."
            });
        }

        return NoContent();
    }

    [HttpPost("picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile? file)
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest(new
            {
                message = "Please select an image file to upload."
            });
        }

        try
        {
            var response = await _profileService.UploadProfilePictureAsync(currentUserId, file);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("picture")]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var userId = await GetCurrentUserIdAsync();

        if (userId is not { } currentUserId)
        {
            return Unauthorized();
        }

        var success = await _profileService.DeleteProfilePictureAsync(currentUserId);
        if (!success)
        {
            return NotFound(new
            {
                message = "Profile not found."
            });
        }

        return NoContent();
    }
}
