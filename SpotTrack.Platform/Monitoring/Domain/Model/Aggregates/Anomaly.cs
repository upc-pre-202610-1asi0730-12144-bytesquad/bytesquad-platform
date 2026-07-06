using SpotTrack.Platform.Monitoring.Domain.Model.Commands;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;

public partial class Anomaly
{
    public int Id { get; private set; }

    public int ReservationId { get; private set; }

    public int EquipmentId { get; private set; }

    public int ZoneId { get; private set; }

    public string AnomalyDescription { get; private set; } = null!;

    public DateTimeOffset EmissionDate { get; private set; }

    private Anomaly() { }

    public Anomaly(ReportAnomalyCommand command)
    {
        if (command.ReservationId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.ReservationId), command.ReservationId,
                "ReservationId must be a positive integer.");
        if (command.EquipmentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.EquipmentId), command.EquipmentId,
                "EquipmentId must be a positive integer.");
        if (command.ZoneId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.ZoneId), command.ZoneId,
                "ZoneId must be a positive integer.");
        if (string.IsNullOrWhiteSpace(command.AnomalyDescription))
            throw new ArgumentException("AnomalyDescription cannot be null or whitespace.", nameof(command.AnomalyDescription));

        ReservationId = command.ReservationId;
        EquipmentId = command.EquipmentId;
        ZoneId = command.ZoneId;
        AnomalyDescription = command.AnomalyDescription;
        EmissionDate = DateTimeOffset.UtcNow;
    }
}
