using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyGymConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Gym>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id).ValueGeneratedOnAdd();

            entity.OwnsOne(g => g.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.Value).IsRequired().HasMaxLength(100).HasColumnName("name");
            });

            entity.OwnsOne(g => g.Address, address =>
            {
                address.WithOwner().HasForeignKey("Id");
                address.Property(a => a.Street).IsRequired().HasMaxLength(200).HasColumnName("street");
                address.Property(a => a.District).IsRequired().HasMaxLength(100).HasColumnName("district");
                address.Property(a => a.City).IsRequired().HasMaxLength(100).HasColumnName("city");
            });

            entity.HasMany(g => g.Branches)
                .WithOne()
                .HasForeignKey("gym_id")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Branch>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).ValueGeneratedOnAdd();
            entity.ToTable("branches");

            entity.OwnsOne(b => b.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.Value).IsRequired().HasMaxLength(100).HasColumnName("name");
            });

            entity.OwnsOne(b => b.Address, address =>
            {
                address.WithOwner().HasForeignKey("Id");
                address.Property(a => a.Street).IsRequired().HasMaxLength(200).HasColumnName("street");
                address.Property(a => a.District).IsRequired().HasMaxLength(100).HasColumnName("district");
                address.Property(a => a.City).IsRequired().HasMaxLength(100).HasColumnName("city");
            });
        });
    }
}
