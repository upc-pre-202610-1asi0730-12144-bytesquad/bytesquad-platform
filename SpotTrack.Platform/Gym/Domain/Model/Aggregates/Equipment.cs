using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Gyms.Domain.Model.Aggregates;

public partial class Equipment
{
    public int Id { get; private set; }

    public EquipmentName Name { get; private set; } = null!;

    public ZoneId ZoneId { get; private set; } = null!;

    public EquipmentStatus Status { get; private set; }

    private Equipment() { }

    public Equipment(RegisterEquipmentCommand command)
    {
        Name = new EquipmentName(command.Name);
        ZoneId = new ZoneId(command.ZoneId);
        Status = EquipmentStatus.Available;
    }

    public void Occupy()
    {
        if (Status is not EquipmentStatus.Available)
            throw new InvalidOperationException(
                $"Cannot occupy equipment that is in '{Status}' status.");

        Status = EquipmentStatus.Occupied;
    }

    public void Release()
    {
        if (Status is not EquipmentStatus.Occupied)
            throw new InvalidOperationException(
                $"Cannot release equipment that is in '{Status}' status.");

        Status = EquipmentStatus.Available;
    }

    public void MarkOutOfService()
    {
        if (Status is EquipmentStatus.OutOfService or EquipmentStatus.Decommissioned)
            throw new InvalidOperationException(
                $"Cannot mark equipment out of service that is already in '{Status}' status.");

        Status = EquipmentStatus.OutOfService;
    }

    public void MarkAvailable()
    {
        if (Status is not (EquipmentStatus.OutOfService or EquipmentStatus.Maintenance))
            throw new InvalidOperationException(
                $"Cannot mark equipment available from '{Status}' status.");

        Status = EquipmentStatus.Available;
    }
}
