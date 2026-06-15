using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Domain.Model.Entities;

namespace SpotTrack.Platform.Reservations.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyReservationsConfiguration(this ModelBuilder builder)
    {
       
        builder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).ValueGeneratedOnAdd();

            entity.Property(r => r.ClientId).IsRequired();
            entity.Property(r => r.EquipmentId).IsRequired();

            
            entity.Property(r => r.StartDate).IsRequired();
            entity.Property(r => r.EndDate).IsRequired();

           
            entity.Ignore(r => r.Period);

          
            entity.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

       
            entity.HasOne(r => r.Request)
                .WithOne()
                .HasForeignKey<ReservationRequest>(rr => rr.ReservationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

    
            entity.Navigation(r => r.Request).HasField("_request");
        });

       
        builder.Entity<ReservationRequest>(entity =>
        {
            entity.HasKey(rr => rr.Id);
            entity.Property(rr => rr.Id).ValueGeneratedOnAdd();

            entity.Property(rr => rr.ReservationId).IsRequired();

            entity.Property(rr => rr.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(40);
        });
    }
}