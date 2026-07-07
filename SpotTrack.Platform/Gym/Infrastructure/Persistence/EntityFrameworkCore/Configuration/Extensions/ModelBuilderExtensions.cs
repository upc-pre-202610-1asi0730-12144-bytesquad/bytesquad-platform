using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyEquipmentConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.ToTable("equipment");

            entity.OwnsOne(e => e.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.Value).IsRequired().HasMaxLength(100).HasColumnName("name");
            });

            entity.Property(e => e.Model).IsRequired().HasMaxLength(100).HasColumnName("model");

            entity.OwnsOne(e => e.ZoneId, zoneId =>
            {
                zoneId.WithOwner().HasForeignKey("Id");
                zoneId.Property(z => z.Value).IsRequired().HasColumnName("zone_id");
            });

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("status")
                .IsRequired();

            entity.Property(e => e.MaintenanceThreshold)
                .HasColumnName("maintenance_threshold")
                .IsRequired(false);
        });
    }

    public static void ApplyGymConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Gym>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id).ValueGeneratedOnAdd();
            entity.Property(g => g.AdminId).IsRequired().HasColumnName("admin_id");

            entity.OwnsOne(g => g.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.Value).IsRequired().HasMaxLength(100).HasColumnName("name");
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

            entity.HasMany(b => b.Zones)
                .WithOne()
                .HasForeignKey(z => z.BranchId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Zone>(entity =>
        {
            entity.HasKey(z => z.Id);
            entity.Property(z => z.Id).ValueGeneratedOnAdd();
            entity.ToTable("zones");
            entity.Property(z => z.MaximumOccupancy).IsRequired().HasColumnName("maximum_occupancy");

            entity.OwnsOne(z => z.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.Value).IsRequired().HasMaxLength(100).HasColumnName("name");
            });
        });

        builder.Entity<AuthorizedDni>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();
            entity.Property(a => a.GymId).IsRequired().HasColumnName("gym_id");
            entity.Property(a => a.Dni).IsRequired().HasMaxLength(8).HasColumnName("dni");
            entity.HasIndex(a => new { a.GymId, a.Dni }).IsUnique();
            entity.ToTable("gym_authorized_dnis");
        });
    }
}
