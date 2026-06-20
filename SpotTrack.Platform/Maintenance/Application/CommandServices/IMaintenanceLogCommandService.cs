using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Maintenances.Application.CommandServices;

public interface IMaintenanceLogCommandService
{
    Task<Result<MaintenanceLog>> Handle(RegisterMaintenanceCompletionCommand command, CancellationToken cancellationToken);
}
