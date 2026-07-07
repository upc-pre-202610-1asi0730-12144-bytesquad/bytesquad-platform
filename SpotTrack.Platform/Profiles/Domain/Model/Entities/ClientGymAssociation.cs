namespace SpotTrack.Platform.Profiles.Domain.Model.Entities;

public class ClientGymAssociation
{
    public int Id { get; private set; }
    public int ClientId { get; private set; }
    public int GymId { get; private set; }
    public bool Active { get; private set; }

    private ClientGymAssociation() { }

    private ClientGymAssociation(int clientId, int gymId, bool active)
    {
        ClientId = clientId;
        GymId = gymId;
        Active = active;
    }

    public static ClientGymAssociation Create(int clientId, int gymId, bool active) =>
        new(clientId, gymId, active);

    public void Activate() => Active = true;
    public void Deactivate() => Active = false;
}
