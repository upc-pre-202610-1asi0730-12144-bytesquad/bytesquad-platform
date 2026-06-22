using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Gyms.Domain.Model.Aggregates;

public partial class Gym
{
    public int Id { get; private set; }

    public GymName Name { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    private List<Branch> _branches = new();
    public IReadOnlyCollection<Branch> Branches => _branches.AsReadOnly();

    private Gym() { }

    public Gym(CreateGymCommand command)
    {
        Name = new GymName(command.Name);
        Address = new Address(command.Street, command.District, command.City);
    }

    public void AddBranch(string name, string street, string district, string city)
    {
        var branchName = new BranchName(name);
        var address = new Address(street, district, city);
        _branches.Add(new Branch(branchName, address));
    }
}
