using FormManagementSystem.Models;

namespace FormManagementSystem.Repositories;

public interface IRegistrationRepository
{
    Task<RegistrationRequest> CreateAsync(
        RegistrationRequest request);

    Task<IEnumerable<RegistrationRequest>> GetAsync(
        RegistrationStatus? status);

    Task<RegistrationRequest?> GetByIdAsync(int id);

    Task UpdateAsync(
        RegistrationRequest request);
}