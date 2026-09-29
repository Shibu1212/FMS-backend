using FormManagementSystem.DTOs.Forms;
using FormManagementSystem.Models;
using FormManagementSystem.Repositories;

namespace FormManagementSystem.Services;

public class FormFieldService : IFormFieldService
{
    private readonly IFormFieldRepository _formFieldRepository;
    private readonly IFormRepository _formRepository;
    private readonly IUserRepository _userRepository;

    public FormFieldService(
        IFormFieldRepository formFieldRepository,
        IFormRepository formRepository,
        IUserRepository userRepository)
    {
        _formFieldRepository = formFieldRepository;
        _formRepository = formRepository;
        _userRepository = userRepository;
    }

    public async Task<FormFieldResponseDto> CreateAsync(
        int formId,
        CreateFormFieldRequestDto dto)
    {
        var form = await _formRepository.GetByIdAsync(formId);

        if (form == null)
            throw new KeyNotFoundException("Form not found.");

        if (!Enum.TryParse<FormFieldType>(
                dto.FieldType,
                true,
                out var fieldType))
        {
            throw new ArgumentException(
                "Invalid field type.");
        }

        var field = new FormField
        {
            FormId = formId,
            Label = dto.Label.Trim(),
            FieldType = fieldType,
            IsRequired = dto.IsRequired,
            DisplayOrder = dto.DisplayOrder,
            Options = dto.Options?.Trim()
        };

        var createdField =
            await _formFieldRepository.CreateAsync(field);

        return await MapToResponseDto(createdField);
    }

    public async Task<IEnumerable<FormFieldResponseDto>> GetByFormIdAsync(
    int formId)
    {
        var fields =
            await _formFieldRepository.GetByFormIdAsync(formId);

        var result = new List<FormFieldResponseDto>();

        foreach (var field in fields)
        {
            result.Add(await MapToResponseDto(field));
        }

        return result;
    }

    public async Task<FormFieldResponseDto?> GetByIdAsync(int id)
    {
        var field =
            await _formFieldRepository.GetByIdAsync(id);

        if (field == null)
            return null;

        return await MapToResponseDto(field);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateFormFieldRequestDto dto)
    {
        if (!Enum.TryParse<FormFieldType>(
                dto.FieldType,
                true,
                out var fieldType))
        {
            throw new ArgumentException(
                "Invalid field type.");
        }

        var existingField =
            await _formFieldRepository.GetByIdAsync(id);

        if (existingField == null)
            return false;

        existingField.Label = dto.Label.Trim();
        existingField.FieldType = fieldType;
        existingField.IsRequired = dto.IsRequired;
        existingField.DisplayOrder = dto.DisplayOrder;
        existingField.Options = dto.Options?.Trim();

        return await _formFieldRepository.UpdateAsync(
            existingField);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _formFieldRepository.DeleteAsync(id);
    }

    private async Task<FormFieldResponseDto> MapToResponseDto(
    FormField field)
    {
        var createdByUser = field.CreatedBy.HasValue
            ? await _userRepository.GetByIdAsync(field.CreatedBy.Value)
            : null;

        var updatedByUser = field.UpdatedBy.HasValue
            ? await _userRepository.GetByIdAsync(field.UpdatedBy.Value)
            : null;

        return new FormFieldResponseDto
        {
            Id = field.Id,
            FormId = field.FormId,
            Label = field.Label,
            FieldType = field.FieldType.ToString(),
            IsRequired = field.IsRequired,
            DisplayOrder = field.DisplayOrder,
            Options = field.Options,

            CreatedAt = field.CreatedAt,
            CreatedBy = field.CreatedBy,
            CreatedByName = createdByUser?.Name,

            UpdatedAt = field.UpdatedAt,
            UpdatedBy = field.UpdatedBy,
            UpdatedByName = updatedByUser?.Name
        };
    }
}