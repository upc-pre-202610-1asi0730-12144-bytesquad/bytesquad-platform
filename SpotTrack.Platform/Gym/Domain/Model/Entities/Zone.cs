using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Gyms.Domain.Model.Entities;

public class Zone
{
    public int Id { get; private set; }

    public ZoneName Name { get; private set; } = null!;

    private Zone() { }

    public Zone(ZoneName name)
    {
        Name = name;
    }
}
