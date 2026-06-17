using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Entities;

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

            entity.Ignore(c => c.ExerciseBlocks);
        });
 
    }
}