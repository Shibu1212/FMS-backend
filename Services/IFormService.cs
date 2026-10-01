using FormManagementSystem.DTOs.Forms;

namespace FormManagementSystem.Services;

using FormManagementSystem.DTOs.Common;

public interface IFormService
{
    Task<FormResponseDto> CreateAsync(CreateFormRequestDto dto);

    Task<PaginatedResponseDto<FormResponseDto>> GetAllAsync(
    PaginationRequestDto request);

    Task<PaginatedResponseDto<FormResponseDto>> GetPublishedAsync(
        PaginationRequestDto request);

    Task<FormResponseDto?> GetByIdAsync(int id);

    Task<FormResponseDto?> GetPublishedByIdAsync(int id);

    Task<bool> UpdateAsync(int id, UpdateFormRequestDto dto);

    Task<bool> DeleteAsync(int id);
}