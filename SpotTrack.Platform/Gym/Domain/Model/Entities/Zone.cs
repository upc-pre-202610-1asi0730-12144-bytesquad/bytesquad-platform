using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Gyms.Domain.Model.Entities;

public class Zone
{
    public int Id { get; private set; }

    public ZoneName Name { get; private set; } = null!;

    public int MaximumOccupancy { get; private set; }

    public int BranchId { get; private set; }

    private Zone() { }

    public Zone(ZoneName name, int maximumOccupancy, int branchId)
    {
        if (maximumOccupancy <= 0)
            throw new ArgumentException("MaximumOccupancy must be greater than zero.", nameof(maximumOccupancy));

        Name = name;
        MaximumOccupancy = maximumOccupancy;
        BranchId = branchId;
    }
}
