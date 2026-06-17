using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Gyms.Domain.Model.Entities;

public class Branch
{
    public int Id { get; private set; }

    public BranchName Name { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    private List<Zone> _zones = new();
    public IReadOnlyCollection<Zone> Zones => _zones.AsReadOnly();

    private Branch() { }

    public Branch(BranchName name, Address address)
    {
        Name = name;
        Address = address;
    }

    public void AddZone(string name)
    {
        var zoneName = new ZoneName(name);
        _zones.Add(new Zone(zoneName));
    }
}
