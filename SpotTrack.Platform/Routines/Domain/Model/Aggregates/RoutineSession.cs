using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Routines.Domain.Model.Aggregates;

public partial class RoutineSession
{
    public int Id { get; private set; }

    public int RoutineId { get; private set; }

    public ClientId ClientId { get; private set; } = null!;

    public RoutineSessionStatus Status { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public int ClientIdValue => ClientId.Value;

    private RoutineSession() { }

    public RoutineSession(StartRoutineCommand command)
    {
        RoutineId = command.RoutineId;
        ClientId = new ClientId(command.ClientId);
        Status = RoutineSessionStatus.Started;
        StartedAt = DateTimeOffset.UtcNow;
    }

    public void Complete() => Status = RoutineSessionStatus.Completed;

    public void MarkMissed() => Status = RoutineSessionStatus.Missed;
}
