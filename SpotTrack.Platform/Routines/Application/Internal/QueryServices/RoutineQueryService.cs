using SpotTrack.Platform.Routines.Application.QueryServices;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Queries;
using SpotTrack.Platform.Routines.Domain.Repositories;

namespace SpotTrack.Platform.Routines.Application.Internal.QueryServices;

public class RoutineQueryService(IRoutineRepository routineRepository) : IRoutineQueryService
{
    public async Task<Routine?> Handle(GetRoutineByIdQuery query, CancellationToken cancellationToken)
        => await routineRepository.FindByIdAsync(query.RoutineId, cancellationToken);

    public async Task<IEnumerable<Routine>> Handle(GetAllRoutinesByClientIdQuery query, CancellationToken cancellationToken)
        => await routineRepository.FindAllByClientIdAsync(query.ClientId, cancellationToken);
}
