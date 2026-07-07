using SpotTrack.Platform.Gyms.Interfaces.Acl;
using SpotTrack.Platform.Memberships.Application.QueryServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Queries;
using SpotTrack.Platform.Profiles.Interfaces.Acl;
using SpotTrack.Platform.Reservations.Interfaces.Acl;

namespace SpotTrack.Platform.Reservations.Application.Acl;

public class ReservationsMembershipContextFacade(
    IGymContextFacade gymContextFacade,
    IProfilesContextFacade profilesContextFacade,
    IMembershipQueryService membershipQueryService) : IReservationsMembershipContextFacade
{
    public async Task<bool> GymHasActiveMembershipAsync(int equipmentId, CancellationToken cancellationToken)
    {
        var adminId = await gymContextFacade.GetAdminIdByEquipmentIdAsync(equipmentId, cancellationToken);
        if (adminId is null) return false;

        var memberships = await membershipQueryService.Handle(
            new GetAllMembershipsByClientIdQuery(adminId.Value), cancellationToken);

        return memberships.Any(m =>
            m.Status == EMembershipStatus.Active ||
            m.Status == EMembershipStatus.PendingCancellation);
    }

    public async Task<bool> ClientGymHasActiveMembershipAsync(int userId, CancellationToken cancellationToken)
    {
        var gymId = await profilesContextFacade.GetActiveGymIdForClientAsync(userId, cancellationToken);
        if (gymId == 0) return false;

        var adminId = await gymContextFacade.GetAdminIdByGymIdAsync(gymId, cancellationToken);
        if (adminId == 0) return false;

        var memberships = await membershipQueryService.Handle(
            new GetAllMembershipsByClientIdQuery(adminId), cancellationToken);

        return memberships.Any(m =>
            m.Status == EMembershipStatus.Active ||
            m.Status == EMembershipStatus.PendingCancellation);
    }
}
