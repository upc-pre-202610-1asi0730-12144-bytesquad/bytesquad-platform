using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Routines.Application.CommandServices;

public interface IRoutineSessionCommandService
{
    Task<Result<RoutineSession>> Handle(StartRoutineCommand command, CancellationToken cancellationToken);
}
