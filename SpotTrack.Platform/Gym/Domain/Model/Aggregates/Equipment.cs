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
}
