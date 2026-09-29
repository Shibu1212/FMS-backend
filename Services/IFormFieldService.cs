using FormManagementSystem.DTOs.Forms;

namespace FormManagementSystem.Services;

public interface IFormFieldService
{
    Task<FormFieldResponseDto> CreateAsync(
        int formId,
        CreateFormFieldRequestDto dto);

    Task<IEnumerable<FormFieldResponseDto>> GetByFormIdAsync(
        int formId);

    Task<FormFieldResponseDto?> GetByIdAsync(
        int id);

    Task<bool> UpdateAsync(
        int id,
        UpdateFormFieldRequestDto dto);

    Task<bool> DeleteAsync(
        int id);
}