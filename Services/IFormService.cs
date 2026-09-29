using FormManagementSystem.DTOs.Forms;

namespace FormManagementSystem.Services;

public interface IFormService
{
    Task<FormResponseDto> CreateAsync(CreateFormRequestDto dto);

    Task<IEnumerable<FormResponseDto>> GetAllAsync();

    Task<IEnumerable<FormResponseDto>> GetPublishedAsync();

    Task<FormResponseDto?> GetByIdAsync(int id);

    Task<FormResponseDto?> GetPublishedByIdAsync(int id);

    Task<bool> UpdateAsync(int id, UpdateFormRequestDto dto);

    Task<bool> DeleteAsync(int id);
}