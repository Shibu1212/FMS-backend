namespace FormManagementSystem.Models;

public class Certification
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string IssuingOrganization { get; set; } = string.Empty;

    public string? CredentialId { get; set; }

    public string? CredentialUrl { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool DoesNotExpire { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
}
