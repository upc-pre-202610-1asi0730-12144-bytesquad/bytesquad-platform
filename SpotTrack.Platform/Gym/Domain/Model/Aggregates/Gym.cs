using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Gyms.Domain.Model.Aggregates;

public partial class Gym
{
    public int Id { get; private set; }

    public GymName Name { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    private Gym() { }

    public Gym(CreateGymCommand command)
    {
        Name = new GymName(command.Name);
        Address = new Address(command.Street, command.District, command.City);
    }
}
