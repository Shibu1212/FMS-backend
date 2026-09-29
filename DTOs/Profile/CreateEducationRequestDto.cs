using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FormManagementSystem.Models;

namespace FormManagementSystem.DTOs.Profile;

public class CreateEducationRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Education type is required.")]
    [EnumDataType(typeof(EducationType), ErrorMessage = "Invalid education type.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EducationType EducationType { get; set; }

    [Required(ErrorMessage = "Degree is required.")]
    [StringLength(100, ErrorMessage = "Degree cannot exceed 100 characters.")]
    public string Degree { get; set; } = string.Empty;

    [Required(ErrorMessage = "Field of study is required.")]
    [StringLength(100, ErrorMessage = "Field of study cannot exceed 100 characters.")]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Institution name is required.")]
    [StringLength(200, ErrorMessage = "Institution name cannot exceed 200 characters.")]
    public string InstitutionName { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "Institution location cannot exceed 150 characters.")]
    public string? InstitutionLocation { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrentlyStudying { get; set; }

    [Required(ErrorMessage = "Grade type is required.")]
    [EnumDataType(typeof(GradeType), ErrorMessage = "Invalid grade type.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GradeType GradeType { get; set; }

    [StringLength(20, ErrorMessage = "Grade cannot exceed 20 characters.")]
    public string? Grade { get; set; }

    [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
    public decimal? Percentage { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "CGPA must be a non-negative value.")]
    public decimal? CGPA { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // A. If IsCurrentlyStudying == true: EndDate must be null
        if (IsCurrentlyStudying && EndDate.HasValue)
        {
            yield return new ValidationResult(
                "End date must be null when currently studying.",
                new[] { nameof(EndDate) });
        }

        // B. If IsCurrentlyStudying == false: EndDate should normally be supplied
        if (!IsCurrentlyStudying && !EndDate.HasValue)
        {
            yield return new ValidationResult(
                "End date is required when not currently studying.",
                new[] { nameof(EndDate) });
        }

        // C. EndDate must not be earlier than StartDate
        if (EndDate.HasValue && EndDate.Value < StartDate)
        {
            yield return new ValidationResult(
                "End date cannot be earlier than start date.",
                new[] { nameof(EndDate) });
        }
    }
}
