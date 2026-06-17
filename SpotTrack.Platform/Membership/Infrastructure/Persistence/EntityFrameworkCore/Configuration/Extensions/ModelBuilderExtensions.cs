using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;

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

            entity.Property(m => m.StartDate).IsRequired();
            entity.Property(m => m.EndDate).IsRequired();

            entity.Ignore(m => m.Period);
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
