using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Gyms.Domain.Model.Events;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Shared.Application.Internal.EventHandlers;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Application.Internal.EventHandlers;

public class GymDniRevokedEventHandler(
    IClientGymAssociationRepository clientGymAssociationRepository,
    IUnitOfWork unitOfWork,
    ILogger<GymDniRevokedEventHandler> logger)
    : IEventHandler<GymDniRevokedEvent>
{
    public async Task Handle(GymDniRevokedEvent notification, CancellationToken cancellationToken)
    {
        var affected = await clientGymAssociationRepository.FindAllByGymIdAndDniAsync(
            notification.GymId, notification.Dni, cancellationToken);

        var associations = affected.ToList();
        if (associations.Count == 0) return;

        foreach (var association in associations)
        {
            association.Deactivate();
            clientGymAssociationRepository.Update(association);
        }

        await unitOfWork.CompleteAsync(cancellationToken);

        logger.LogInformation(
            "GymDniRevoked: deactivated {Count} association(s) for gym {GymId} and DNI {Dni}",
            associations.Count, notification.GymId, notification.Dni);
    }
}
