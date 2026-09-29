using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using FormManagementSystem.Models;
using FormManagementSystem.Extensions;

namespace FormManagementSystem.Data;

public class AppDbContext : DbContext
{
    // Gives us access to the current logged-in user's JWT claims
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        // Get the PublicId of the currently logged-in user from JWT
        var currentUserPublicId =
            _httpContextAccessor.HttpContext?.User.GetPublicId();

        // Resolve PublicId to the internal database User.Id
        int? currentUserId = null;

        if (currentUserPublicId.HasValue)
        {
            currentUserId = Users
                .AsNoTracking()
                .Where(u => u.PublicId == currentUserPublicId.Value)
                .Select(u => (int?)u.Id)
                .FirstOrDefault();
        }

        // Use UTC for database timestamps
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            // ==========================================
            // NEW RECORD
            // ==========================================
            if (entry.State == EntityState.Added)
            {
                var createdAtProp = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == "CreatedAt");

                if (createdAtProp != null &&
                    createdAtProp.Metadata.ClrType == typeof(DateTime))
                {
                    createdAtProp.CurrentValue = now;
                }

                var createdByProp = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == "CreatedBy");

                if (createdByProp != null)
                {
                    createdByProp.CurrentValue = currentUserId;
                }
            }

            // ==========================================
            // EXISTING RECORD BEING UPDATED
            // ==========================================
            else if (entry.State == EntityState.Modified)
            {
                var updatedAtProp = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");

                if (updatedAtProp != null)
                {
                    updatedAtProp.CurrentValue = now;
                }

                var updatedByProp = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == "UpdatedBy");

                if (updatedByProp != null)
                {
                    updatedByProp.CurrentValue = currentUserId;
                }

                // CreatedAt must NEVER change after creation
                var createdAtProp = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == "CreatedAt");

                if (createdAtProp != null)
                {
                    createdAtProp.IsModified = false;
                }

                // CreatedBy must NEVER change after creation
                var createdByProp = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == "CreatedBy");

                if (createdByProp != null)
                {
                    createdByProp.IsModified = false;
                }
            }
        }
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<Education> Educations { get; set; }

    public DbSet<Experience> Experiences { get; set; }

    public DbSet<Certification> Certifications { get; set; }

    public DbSet<RegistrationRequest> RegistrationRequests { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public DbSet<Form> Forms { get; set; }

    public DbSet<FormField> FormFields { get; set; }

    public DbSet<FormResponse> FormResponses { get; set; }

    public DbSet<FormResponseValue> FormResponseValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User → Email must be unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PublicId)
            .IsUnique();

        // RegistrationRequest → Status stored as string
        modelBuilder.Entity<RegistrationRequest>()
            .Property(r => r.Status)
            .HasConversion<string>();

        // User → UserProfile (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // UserProfile → Educations (One-to-Many)
        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasOne(e => e.UserProfile)
                .WithMany(p => p.Educations)
                .HasForeignKey(e => e.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.EducationType)
                .HasConversion<string>();

            entity.Property(e => e.GradeType)
                .HasConversion<string>();

            entity.Property(e => e.Percentage)
                .HasPrecision(5, 2);

            entity.Property(e => e.CGPA)
                .HasPrecision(4, 2);
        });

        // UserProfile → Experiences (One-to-Many)
        modelBuilder.Entity<Experience>(entity =>
        {
            entity.HasOne(e => e.UserProfile)
                .WithMany(p => p.Experiences)
                .HasForeignKey(e => e.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.EmploymentType)
                .HasConversion<string>();
        });

        // UserProfile → Certifications (One-to-Many)
        modelBuilder.Entity<Certification>(entity =>
        {
            entity.HasOne(c => c.UserProfile)
                .WithMany(p => p.Certifications)
                .HasForeignKey(c => c.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // User → RefreshTokens (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany<RefreshToken>()
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User → PasswordResetTokens (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany<PasswordResetToken>()
            .WithOne(prt => prt.User)
            .HasForeignKey(prt => prt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User → UserRoles
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Role → UserRoles
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PasswordResetToken>()
            .HasOne(prt => prt.User)
            .WithMany(u => u.PasswordResetTokens)
            .HasForeignKey(prt => prt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PasswordResetToken>()
            .HasIndex(prt => prt.TokenHash)
            .IsUnique();

        modelBuilder.Entity<Form>()
            .Property(f => f.Status)
            .HasConversion<string>();

        modelBuilder.Entity<FormField>()
            .Property(field => field.FieldType)
            .HasConversion<string>();

        modelBuilder.Entity<FormField>()
           .HasOne(field => field.Form)
           .WithMany(form => form.Fields)
           .HasForeignKey(field => field.FormId)
           .OnDelete(DeleteBehavior.Cascade);

        // Form -> FormResponse
        modelBuilder.Entity<FormResponse>()
            .HasOne(response => response.Form)
            .WithMany()
            .HasForeignKey(response => response.FormId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> FormResponse
        modelBuilder.Entity<FormResponse>()
            .HasOne(response => response.SubmittedByUser)
            .WithMany()
            .HasForeignKey(response => response.SubmittedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // FormResponse -> FormResponseValue
        modelBuilder.Entity<FormResponseValue>()
            .HasOne(value => value.FormResponse)
            .WithMany(response => response.Values)
            .HasForeignKey(value => value.FormResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        // FormField -> FormResponseValue
        modelBuilder.Entity<FormResponseValue>()
            .HasOne(value => value.FormField)
            .WithMany()
            .HasForeignKey(value => value.FormFieldId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}