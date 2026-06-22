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

        builder.Entity<RoutineSession>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();

            entity.Property(s => s.RoutineId).IsRequired().HasColumnName("routine_id");

            entity.OwnsOne(s => s.ClientId, clientId =>
            {
                clientId.WithOwner().HasForeignKey("Id");
                clientId.Property(c => c.Value).IsRequired().HasColumnName("client_id");
                clientId.HasIndex(c => c.Value).HasDatabaseName("ix_routine_sessions_client_id");
            });

            entity.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("status")
                .IsRequired();

            entity.Property(s => s.StartedAt).IsRequired().HasColumnName("started_at");

            entity.Ignore(s => s.ClientIdValue);
        });
    }
}
