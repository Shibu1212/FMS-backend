using FormManagementSystem.DTOs.FormResponse;
using FormManagementSystem.DTOs.Common;

namespace FormManagementSystem.Services;

public interface IFormResponseService
{
    Task<FormResponseResponseDto> CreateAsync(
        CreateFormResponseRequestDto request,
        int userId);

    Task<FormResponseResponseDto?> GetByIdAsync(int id);

    Task<PaginatedResponseDto<FormResponseResponseDto>> GetByUserIdAsync(
    int userId,
    PaginationRequestDto request);

    Task<PaginatedResponseDto<FormResponseResponseDto>> GetByFormIdAsync(
        int formId,
        PaginationRequestDto request);


}