using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Memberships.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMembershipsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Membership>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).ValueGeneratedOnAdd();

            entity.Property(m => m.ClientId).IsRequired();

            entity.Property(m => m.Plan)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(m => m.PendingDowngradePlan)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(m => m.StartDate).IsRequired();
            entity.Property(m => m.EndDate).IsRequired();

            entity.Ignore(m => m.Period);
        });

        builder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId);
            entity.Property(p => p.PaymentId).HasColumnName("payment_id").ValueGeneratedNever();
            entity.Property(p => p.UserId).HasColumnName("user_id");
            entity.Property(p => p.PendingRegistrationId).HasColumnName("pending_registration_id");
            entity.Property(p => p.MembershipId).HasColumnName("membership_id");
            entity.Property(p => p.MembershipPlan)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("membership_plan");
            entity.Property(p => p.Amount).IsRequired().HasColumnType("decimal(10,2)").HasColumnName("amount");
            entity.Property(p => p.Currency).IsRequired().HasMaxLength(3).HasColumnName("currency");
            entity.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(p => p.GatewayTransactionId).HasMaxLength(200).HasColumnName("gateway_transaction_id");
            entity.Property(p => p.Purpose)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30)
                .HasColumnName("purpose");
        });

        builder.Entity<BranchAccess>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).ValueGeneratedOnAdd();

            entity.Property(b => b.MembershipId).IsRequired();
            entity.Property(b => b.BranchId).IsRequired();
            entity.Property(b => b.GrantedByAdminId).IsRequired();

            entity.Property(b => b.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10);
        });
    }
}
