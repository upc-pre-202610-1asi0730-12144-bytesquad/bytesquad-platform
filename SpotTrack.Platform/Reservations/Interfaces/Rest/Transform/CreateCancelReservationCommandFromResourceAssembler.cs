using SpotTrack.Platform.Reservations.Domain.Model.Commands;
using SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Transform;

public static class CreateCancelReservationCommandFromResourceAssembler
{
    public static CreateCancelReservationCommand ToCommandResource(
        CreateCancelReservationResource resource) => new(resource.ReservationId);
}