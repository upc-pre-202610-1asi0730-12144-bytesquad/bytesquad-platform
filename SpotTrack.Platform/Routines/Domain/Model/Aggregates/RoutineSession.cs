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

    public int ClientIdValue => ClientId.Value;

    private List<SessionExerciseCompletion> _completedExercises = new();
    public IReadOnlyCollection<SessionExerciseCompletion> CompletedExercises => _completedExercises.AsReadOnly();

    private RoutineSession() { }

    public RoutineSession(StartRoutineCommand command)
    {
        RoutineId = command.RoutineId;
        ClientId = new ClientId(command.ClientId);
        Status = RoutineSessionStatus.Started;
        StartedAt = DateTimeOffset.UtcNow;
    }

    public void Complete()
    {
        if (Status is not RoutineSessionStatus.Started)
            throw new InvalidOperationException(
                $"Cannot complete a routine session in '{Status}' status.");

        Status = RoutineSessionStatus.Completed;
    }

    public void MarkMissed()
    {
        if (Status is not RoutineSessionStatus.Started)
            throw new InvalidOperationException(
                $"Cannot mark a routine session as missed from '{Status}' status.");

        Status = RoutineSessionStatus.Missed;
    }

    public void SetExerciseCompletion(int exerciseBlockId, bool completed)
    {
        if (Status is not RoutineSessionStatus.Started)
            throw new InvalidOperationException(
                $"Cannot change exercise completion for a routine session in '{Status}' status.");

        var existing = _completedExercises.FirstOrDefault(c => c.ExerciseBlockId == exerciseBlockId);
        if (completed && existing is null)
            _completedExercises.Add(new SessionExerciseCompletion(exerciseBlockId));
        else if (!completed && existing is not null)
            _completedExercises.Remove(existing);
    }
}
