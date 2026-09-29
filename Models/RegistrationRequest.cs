namespace FormManagementSystem.Models;

public enum RegistrationStatus
{
    Pending,
    Approved,
    Rejected
}

public class RegistrationRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public RegistrationStatus Status { get; set; }
        = RegistrationStatus.Pending;

    public DateTime RequestedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public int? ReviewedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}