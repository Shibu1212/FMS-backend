using FormManagementSystem.DTOs.FormResponse;
using FormManagementSystem.Models;
using FormManagementSystem.Repositories;

namespace FormManagementSystem.Services;

public class FormResponseService : IFormResponseService
{
    private readonly IFormResponseRepository _formResponseRepository;
    private readonly IFormRepository _formRepository;
    private readonly IFormFieldRepository _formFieldRepository;

    public FormResponseService(
        IFormResponseRepository formResponseRepository,
        IFormRepository formRepository,
        IFormFieldRepository formFieldRepository)
    {
        _formResponseRepository = formResponseRepository;
        _formRepository = formRepository;
        _formFieldRepository = formFieldRepository;
    }

    public async Task<FormResponseResponseDto> CreateAsync(
        CreateFormResponseRequestDto request,
        int userId)
    {
        var form = await _formRepository.GetByIdAsync(request.FormId);

        if (form == null)
        {
            throw new KeyNotFoundException("Form not found.");
        }

        if (form.Status != FormStatus.PUBLISHED)
        {
            throw new InvalidOperationException(
                "Only published forms can be submitted.");
        }

        var alreadySubmitted =
            await _formResponseRepository.ExistsByFormAndUserAsync(
                request.FormId,
                userId);

        if (alreadySubmitted)
        {
            throw new InvalidOperationException(
                "You have already submitted this form.");
        }

        var fields = await _formFieldRepository
            .GetByFormIdAsync(request.FormId);

        var submittedFieldIds = request.Values
            .Select(value => value.FormFieldId)
            .ToHashSet();

        foreach (var field in fields)
        {
            if (field.IsRequired &&
                !submittedFieldIds.Contains(field.Id))
            {
                throw new InvalidOperationException(
                    $"Required field '{field.Label}' is missing.");
            }
        }

        foreach (var value in request.Values)
        {
            var fieldExists = fields.Any(
                field => field.Id == value.FormFieldId);

            if (!fieldExists)
            {
                throw new InvalidOperationException(
                    $"Field with ID {value.FormFieldId} does not belong to this form.");
            }
        }

        var response = new FormResponse
        {
            FormId = request.FormId,
            SubmittedBy = userId,
            SubmittedAt = DateTime.UtcNow,

            Values = request.Values
                .Select(value => new FormResponseValue
                {
                    FormFieldId = value.FormFieldId,
                    Value = value.Value
                })
                .ToList()
        };

        var createdResponse =
            await _formResponseRepository.CreateAsync(response);

        return MapToResponseDto(createdResponse);
    }

    public async Task<FormResponseResponseDto?> GetByIdAsync(int id)
    {
        var response =
            await _formResponseRepository.GetByIdAsync(id);

        if (response == null)
        {
            return null;
        }

        return MapToResponseDto(response);
    }

    public async Task<List<FormResponseResponseDto>> GetByUserIdAsync(
        int userId)
    {
        var responses =
            await _formResponseRepository.GetByUserIdAsync(userId);

        return responses
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<List<FormResponseResponseDto>> GetByFormIdAsync(
    int formId,
    string? search = null)
    {
        var responses =
            await _formResponseRepository.GetByFormIdAsync(
                formId,
                search);

        return responses
            .Select(MapToResponseDto)
            .ToList();
    }

    

    private FormResponseResponseDto MapToResponseDto(
        FormResponse response)
    {
        return new FormResponseResponseDto
        {
            Id = response.Id,
            FormId = response.FormId,
            FormName = response.Form?.Name,

            SubmittedBy = response.SubmittedBy,
            SubmittedByName = response.SubmittedByUser?.Name,
            SubmittedByEmail = response.SubmittedByUser?.Email,

            SubmittedAt = response.SubmittedAt,

            Values = response.Values
                .OrderBy(value => value.FormField?.DisplayOrder)
                .Select(value => new FormResponseValueDto
                {
                    FormFieldId = value.FormFieldId,
                    Value = value.Value
                })
                .ToList()
        };
    }
}