namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

public record ClientGymAssociationResource(int Id, int ClientId, int GymId, bool Active);
