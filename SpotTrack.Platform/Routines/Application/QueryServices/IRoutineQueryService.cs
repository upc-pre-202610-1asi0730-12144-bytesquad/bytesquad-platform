using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Queries;

namespace SpotTrack.Platform.Routines.Application.QueryServices;

public interface IRoutineQueryService
{
    Task<Routine?> Handle(GetRoutineByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Routine>> Handle(GetAllRoutinesByClientIdQuery query, CancellationToken cancellationToken);
}
