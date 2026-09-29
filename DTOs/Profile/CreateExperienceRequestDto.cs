using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FormManagementSystem.Models;

namespace FormManagementSystem.DTOs.Profile;

public class CreateExperienceRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job title is required.")]
    [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
    public string JobTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employment type is required.")]
    [EnumDataType(typeof(EmploymentType), ErrorMessage = "Invalid employment type.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EmploymentType EmploymentType { get; set; }

    [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters.")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrent { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // A. If IsCurrent == true: EndDate must be null
        if (IsCurrent && EndDate.HasValue)
        {
            yield return new ValidationResult(
                "End date must be null when currently working here.",
                new[] { nameof(EndDate) });
        }

        // B. If IsCurrent == false: EndDate should normally be supplied
        if (!IsCurrent && !EndDate.HasValue)
        {
            yield return new ValidationResult(
                "End date is required when not current employment.",
                new[] { nameof(EndDate) });
        }

        // C. If EndDate is supplied: EndDate must not be earlier than StartDate
        if (EndDate.HasValue && EndDate.Value < StartDate)
        {
            yield return new ValidationResult(
                "End date cannot be earlier than start date.",
                new[] { nameof(EndDate) });
        }
    }
}
