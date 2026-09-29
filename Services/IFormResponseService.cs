using FormManagementSystem.DTOs.FormResponse;

namespace FormManagementSystem.Services;

public interface IFormResponseService
{
    Task<FormResponseResponseDto> CreateAsync(
        CreateFormResponseRequestDto request,
        int userId);

    Task<FormResponseResponseDto?> GetByIdAsync(int id);

    Task<List<FormResponseResponseDto>> GetByUserIdAsync(int userId);

    Task<List<FormResponseResponseDto>> GetByFormIdAsync(
    int formId,
    string? search = null);

    
}