using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.UserId).IsRequired();

            entity.OwnsOne(c => c.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.FirstName).HasMaxLength(50).HasColumnName("first_name");
                name.Property(n => n.LastName).HasMaxLength(50).HasColumnName("last_name");
            });
            entity.Navigation(c => c.Name).IsRequired(false);

            entity.OwnsOne(c => c.Email, email =>
            {
                email.WithOwner().HasForeignKey("Id");
                email.Property(e => e.Address).HasMaxLength(100).HasColumnName("email");
                email.HasIndex(e => e.Address).IsUnique();
            });
            entity.Navigation(c => c.Email).IsRequired(false);

            entity.OwnsOne(c => c.Phone, phone =>
            {
                phone.WithOwner().HasForeignKey("Id");
                phone.Property(p => p.Number).HasMaxLength(15).HasColumnName("phone_number");
            });
            entity.Navigation(c => c.Phone).IsRequired(false);

            entity.OwnsOne(c => c.Dni, dni =>
            {
                dni.WithOwner().HasForeignKey("Id");
                dni.Property(d => d.Value).HasMaxLength(8).HasColumnName("dni");
            });
            entity.Navigation(c => c.Dni).IsRequired(false);
        });

        builder.Entity<Admin>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();
            entity.Property(a => a.UserId).IsRequired();

            entity.OwnsOne(a => a.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.FirstName).HasMaxLength(50).HasColumnName("first_name");
                name.Property(n => n.LastName).HasMaxLength(50).HasColumnName("last_name");
            });
            entity.Navigation(a => a.Name).IsRequired(false);

            entity.OwnsOne(a => a.Email, email =>
            {
                email.WithOwner().HasForeignKey("Id");
                email.Property(e => e.Address).HasMaxLength(100).HasColumnName("email");
                email.HasIndex(e => e.Address).IsUnique();
            });
            entity.Navigation(a => a.Email).IsRequired(false);

            entity.OwnsOne(a => a.Phone, phone =>
            {
                phone.WithOwner().HasForeignKey("Id");
                phone.Property(p => p.Number).HasMaxLength(15).HasColumnName("phone_number");
            });
            entity.Navigation(a => a.Phone).IsRequired(false);

            entity.OwnsOne(a => a.Dni, dni =>
            {
                dni.WithOwner().HasForeignKey("Id");
                dni.Property(d => d.Value).HasMaxLength(8).HasColumnName("dni");
            });
            entity.Navigation(a => a.Dni).IsRequired(false);
        });
    }
}
