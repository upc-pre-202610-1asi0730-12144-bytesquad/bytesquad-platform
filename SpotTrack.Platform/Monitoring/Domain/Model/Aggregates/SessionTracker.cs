using SpotTrack.Platform.Monitoring.Domain.Model.Commands;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;

public class SessionTracker
{
    public int Id { get; private set; }
    public int EquipmentId { get; private set; }
    public int AdminId { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? EndedAt { get; private set; }
    public bool IsActive => EndedAt is null;
    public double ElapsedSeconds => (EndedAt ?? DateTimeOffset.UtcNow).Subtract(StartedAt).TotalSeconds;

    private SessionTracker() { }

    public SessionTracker(CreateSessionTrackerCommand command)
    {
        if (command.EquipmentId <= 0)
            throw new ArgumentException("EquipmentId must be positive.", nameof(command.EquipmentId));
        if (command.AdminId <= 0)
            throw new ArgumentException("AdminId must be positive.", nameof(command.AdminId));

        EquipmentId = command.EquipmentId;
        AdminId = command.AdminId;
        StartedAt = command.StartedAt;
    }

    public void End()
    {
        if (!IsActive)
            throw new InvalidOperationException("Session tracker is already ended.");

        EndedAt = DateTimeOffset.UtcNow;
    }
}
