using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Queries;

namespace SpotTrack.Platform.Routines.Application.QueryServices;

public interface IRoutineSessionQueryService
{
    Task<RoutineSession?> Handle(GetRoutineSessionByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<RoutineSession>> Handle(GetAllRoutineSessionsByClientIdQuery query, CancellationToken cancellationToken);
}
