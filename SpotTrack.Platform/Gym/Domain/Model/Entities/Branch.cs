using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Gyms.Domain.Model.Entities;

public class Branch
{
    public int Id { get; private set; }

    public BranchName Name { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    private Branch() { }

    public Branch(BranchName name, Address address)
    {
        Name = name;
        Address = address;
    }
}
