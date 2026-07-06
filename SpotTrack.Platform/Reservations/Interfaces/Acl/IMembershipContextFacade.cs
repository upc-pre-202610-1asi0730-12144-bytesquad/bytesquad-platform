namespace SpotTrack.Platform.Reservations.Interfaces.Acl;

public interface IMembershipContextFacade
{
    Task<bool> GymHasActiveMembershipAsync(int equipmentId, CancellationToken cancellationToken);
}
