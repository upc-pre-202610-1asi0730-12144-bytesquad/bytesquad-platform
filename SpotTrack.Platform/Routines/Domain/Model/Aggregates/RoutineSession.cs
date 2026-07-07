using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Domain.Model.Entities;
using SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Routines.Domain.Model.Aggregates;

public partial class RoutineSession
{
    public int Id { get; private set; }

    public int RoutineId { get; private set; }

    public ClientId ClientId { get; private set; } = null!;

    public RoutineSessionStatus Status { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public ICollection<SessionBlockCompletion> CompletedBlocks { get; private set; } = new List<SessionBlockCompletion>();

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

    public void SetBlockCompleted(int exerciseBlockId, bool isCompleted)
    {
        if (isCompleted)
        {
            if (!CompletedBlocks.Any(b => b.ExerciseBlockId == exerciseBlockId))
                CompletedBlocks.Add(new SessionBlockCompletion(exerciseBlockId));
        }
        else
        {
            var existing = CompletedBlocks.FirstOrDefault(b => b.ExerciseBlockId == exerciseBlockId);
            if (existing is not null)
                CompletedBlocks.Remove(existing);
        }
    }
}
