using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Routines.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyRoutinesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Routine>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.OwnsOne(c => c.Name, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(n => n.Value).IsRequired().HasMaxLength(100).HasColumnName("routine_name");
            });

            entity.OwnsOne(c => c.ClientId, name =>
            {
                name.WithOwner().HasForeignKey("Id");
                name.Property(c => c.Value).IsRequired().HasColumnName("client_id");
                name.HasIndex(c => c.Value).HasDatabaseName("ix_routines_client_id");
            });

            entity.OwnsMany(c => c.ExerciseBlocks, eb =>
            {
                eb.ToTable("exercise_blocks");
                eb.WithOwner().HasForeignKey("routine_id");
                eb.HasKey(b => b.Id);
                eb.Property(b => b.Id).ValueGeneratedOnAdd().HasColumnName("id");

                eb.OwnsOne(b => b.Name, name =>
                {
                    name.WithOwner().HasForeignKey("Id");
                    name.Property(n => n.Value)
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnName("exercise_name");
                });

                eb.Property(b => b.Type)
                    .HasConversion<string>()
                    .HasColumnName("exercise_type")
                    .IsRequired();

                eb.Property(b => b.Order)
                    .HasColumnName("block_order")
                    .IsRequired();
            });
        });
    }
}
