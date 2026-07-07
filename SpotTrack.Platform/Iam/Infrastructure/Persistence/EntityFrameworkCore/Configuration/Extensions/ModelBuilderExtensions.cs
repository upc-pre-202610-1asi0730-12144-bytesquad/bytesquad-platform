using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50).HasColumnName("username");
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired().HasColumnName("password_hash");
            entity.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("role");
            entity.Property(u => u.PasswordResetCodeHash)
                .IsRequired(false)
                .HasColumnName("password_reset_code_hash");
            entity.Property(u => u.PasswordResetExpiresAt)
                .IsRequired(false)
                .HasColumnName("password_reset_expires_at");
        });

        builder.Entity<PendingRegistration>(entity =>
        {
            entity.HasKey(r => r.RegistrationId);
            entity.Property(r => r.RegistrationId).HasColumnName("registration_id").ValueGeneratedNever();
            entity.Property(r => r.Email).IsRequired().HasMaxLength(100).HasColumnName("email");
            entity.HasIndex(r => r.Email);
            entity.Property(r => r.HashedPassword).IsRequired().HasColumnName("hashed_password");
            entity.Property(r => r.FirstName).IsRequired().HasMaxLength(50).HasColumnName("first_name");
            entity.Property(r => r.LastName).IsRequired().HasMaxLength(50).HasColumnName("last_name");
            entity.Property(r => r.PhoneNumber).IsRequired().HasMaxLength(20).HasColumnName("phone_number");
            entity.Property(r => r.Dni).IsRequired().HasMaxLength(20).HasColumnName("dni");
            entity.Property(r => r.CompanyName).IsRequired().HasMaxLength(100).HasColumnName("company_name");
            entity.Property(r => r.Ruc).IsRequired().HasMaxLength(20).HasColumnName("ruc");
            entity.Property(r => r.LegalStructure).IsRequired().HasMaxLength(50).HasColumnName("legal_structure");
            entity.Property(r => r.CompanyPhone).HasMaxLength(20).HasColumnName("company_phone");
            entity.Property(r => r.CompanyEmail).HasMaxLength(100).HasColumnName("company_email");
            entity.Property(r => r.StreetAddress).IsRequired().HasMaxLength(200).HasColumnName("street_address");
            entity.Property(r => r.City).IsRequired().HasMaxLength(100).HasColumnName("city");
            entity.Property(r => r.District).IsRequired().HasMaxLength(100).HasColumnName("district");
            entity.Property(r => r.MembershipTier).IsRequired().HasMaxLength(20).HasColumnName("membership_tier");
            entity.Property(r => r.CreatedAt).IsRequired().HasColumnName("created_at");
            entity.Property(r => r.ExpiresAt).IsRequired().HasColumnName("expires_at");
            entity.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("status");
        });
    }
}
