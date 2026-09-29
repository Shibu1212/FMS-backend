using System.ComponentModel.DataAnnotations;

namespace FormManagementSystem.DTOs.Profile;

public class CreateCertificationRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Issuing organization is required.")]
    [StringLength(200, ErrorMessage = "Issuing organization cannot exceed 200 characters.")]
    public string IssuingOrganization { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "Credential ID cannot exceed 150 characters.")]
    public string? CredentialId { get; set; }

    [StringLength(500, ErrorMessage = "Credential URL cannot exceed 500 characters.")]
    public string? CredentialUrl { get; set; }

    [Required(ErrorMessage = "Issue date is required.")]
    public DateTime IssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool DoesNotExpire { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // A. If CredentialUrl is supplied: validate absolute URL
        if (!string.IsNullOrWhiteSpace(CredentialUrl))
        {
            if (!Uri.TryCreate(CredentialUrl, UriKind.Absolute, out var uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                yield return new ValidationResult(
                    "Credential URL must be a valid absolute URL.",
                    new[] { nameof(CredentialUrl) });
            }
        }

        // B. If DoesNotExpire == true: ExpirationDate must be null
        if (DoesNotExpire && ExpirationDate.HasValue)
        {
            yield return new ValidationResult(
                "Expiration date must be null when certification does not expire.",
                new[] { nameof(ExpirationDate) });
        }

        // C. If DoesNotExpire == false: ExpirationDate must be provided
        if (!DoesNotExpire && !ExpirationDate.HasValue)
        {
            yield return new ValidationResult(
                "Expiration date is required when certification expires.",
                new[] { nameof(ExpirationDate) });
        }

        // D. If ExpirationDate is supplied: ExpirationDate cannot be earlier than IssueDate
        if (ExpirationDate.HasValue && ExpirationDate.Value < IssueDate)
        {
            yield return new ValidationResult(
                "Expiration date cannot be earlier than issue date.",
                new[] { nameof(ExpirationDate) });
        }
    }
}
