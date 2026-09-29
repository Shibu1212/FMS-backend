using FormManagementSystem.DTOs.Registration;
using FormManagementSystem.Models;

namespace FormManagementSystem.Services;

public interface IRegistrationService
{
    Task<RegistrationRequest> CreateAsync(
        RegistrationRequestDto dto);

    Task<IEnumerable<RegistrationResponseDto>> GetAsync(
        RegistrationStatus? status);

    Task<RegistrationResponseDto?> GetByIdAsync(int id);

    Task<bool> UpdateStatusAsync(
        int id,
        RegistrationStatusDto dto,
        int adminUserId);
}