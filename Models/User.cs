namespace FormManagementSystem.Models;

public class User
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public UserProfile UserProfile { get; set; } = null!;

    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
    = new List<PasswordResetToken>();

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}