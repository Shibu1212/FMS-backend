using FormManagementSystem.DTOs.Forms;
using FormManagementSystem.Models;
using FormManagementSystem.Repositories;
using FormManagementSystem.DTOs.Common;

namespace FormManagementSystem.Services;

public class FormService : IFormService
{
    private readonly IFormRepository _formRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFormFieldRepository _formFieldRepository;


    public FormService(
    IFormRepository formRepository,
    IUserRepository userRepository,
    IFormFieldRepository formFieldRepository)
    {
        _formRepository = formRepository;
        _userRepository = userRepository;
        _formFieldRepository = formFieldRepository;
    }

    public async Task<FormResponseDto> CreateAsync(
        CreateFormRequestDto dto)
    {
        var form = new Form
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            Status = FormStatus.DRAFT
        };

        var createdForm = await _formRepository.CreateAsync(form);

        return await MapToResponseDto(createdForm);
    }

    public async Task<PaginatedResponseDto<FormResponseDto>> GetAllAsync(
    PaginationRequestDto request)
    {
        var result = await _formRepository.GetAllAsync(request);

        var formDtos = new List<FormResponseDto>();

        foreach (var form in result.Items)
        {
            formDtos.Add(
                await MapToResponseDto(form));
        }

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        return new PaginatedResponseDto<FormResponseDto>
        {
            Items = formDtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<PaginatedResponseDto<FormResponseDto>> GetPublishedAsync(
    PaginationRequestDto request)
    {
        var result =
            await _formRepository.GetPublishedAsync(request);

        var formDtos = new List<FormResponseDto>();

        foreach (var form in result.Items)
        {
            formDtos.Add(
                await MapToResponseDto(form));
        }

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        return new PaginatedResponseDto<FormResponseDto>
        {
            Items = formDtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<FormResponseDto?> GetByIdAsync(int id)
    {
        var form = await _formRepository.GetByIdAsync(id);

        if (form == null)
            return null;

        return await MapToResponseDto(form);
    }

    public async Task<FormResponseDto?> GetPublishedByIdAsync(int id)
    {
        var form = await _formRepository.GetPublishedByIdAsync(id);

        if (form == null)
            return null;

        return await MapToResponseDto(form);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateFormRequestDto dto)
    {
        if (!Enum.TryParse<FormStatus>(
                dto.Status,
                true,
                out var status))
        {
            throw new ArgumentException(
                "Invalid form status.");
        }

        var existingForm = await _formRepository.GetByIdAsync(id);

        if (existingForm == null)
            return false;

        existingForm.Name = dto.Name.Trim();
        existingForm.Description = dto.Description?.Trim();
        existingForm.Status = status;

        return await _formRepository.UpdateAsync(existingForm);
    }



    public async Task<bool> DeleteAsync(int id)
    {
        return await _formRepository.DeleteAsync(id);
    }

    private async Task<FormResponseDto> MapToResponseDto(Form form)
    {
        var createdByUser = form.CreatedBy.HasValue
            ? await _userRepository.GetByIdAsync(form.CreatedBy.Value)
            : null;

        var updatedByUser = form.UpdatedBy.HasValue
            ? await _userRepository.GetByIdAsync(form.UpdatedBy.Value)
            : null;

        var fields =
            await _formFieldRepository.GetByFormIdAsync(form.Id);

        var fieldDtos = new List<FormFieldResponseDto>();

        foreach (var field in fields)
        {
            var fieldCreatedByUser = field.CreatedBy.HasValue
                ? await _userRepository.GetByIdAsync(field.CreatedBy.Value)
                : null;

            var fieldUpdatedByUser = field.UpdatedBy.HasValue
                ? await _userRepository.GetByIdAsync(field.UpdatedBy.Value)
                : null;

            fieldDtos.Add(new FormFieldResponseDto
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
                CreatedByName = fieldCreatedByUser?.Name,

                UpdatedAt = field.UpdatedAt,
                UpdatedBy = field.UpdatedBy,
                UpdatedByName = fieldUpdatedByUser?.Name
            });
        }

        return new FormResponseDto
        {
            Id = form.Id,
            Name = form.Name,
            Description = form.Description,
            Status = form.Status.ToString(),

            CreatedAt = form.CreatedAt,
            CreatedBy = form.CreatedBy,
            CreatedByName = createdByUser?.Name,

            UpdatedAt = form.UpdatedAt,
            UpdatedBy = form.UpdatedBy,
            UpdatedByName = updatedByUser?.Name,

            Fields = fieldDtos
        };
    }
}