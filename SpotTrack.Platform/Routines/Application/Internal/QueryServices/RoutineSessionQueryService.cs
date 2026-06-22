using SpotTrack.Platform.Routines.Application.QueryServices;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Queries;
using SpotTrack.Platform.Routines.Domain.Repositories;

namespace SpotTrack.Platform.Routines.Application.Internal.QueryServices;

public class RoutineSessionQueryService(IRoutineSessionRepository routineSessionRepository)
    : IRoutineSessionQueryService
{
    public async Task<RoutineSession?> Handle(GetRoutineSessionByIdQuery query, CancellationToken cancellationToken)
        => await routineSessionRepository.FindByIdAsync(query.RoutineSessionId, cancellationToken);

    public async Task<IEnumerable<RoutineSession>> Handle(GetAllRoutineSessionsByClientIdQuery query, CancellationToken cancellationToken)
        => await routineSessionRepository.FindAllByClientIdAsync(query.ClientId, cancellationToken);
}
