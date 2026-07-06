namespace SpotTrack.Platform.Reservations.Interfaces.Acl;

public interface IReservationsMembershipContextFacade
{
    Task<bool> GymHasActiveMembershipAsync(int equipmentId, CancellationToken cancellationToken);
}
