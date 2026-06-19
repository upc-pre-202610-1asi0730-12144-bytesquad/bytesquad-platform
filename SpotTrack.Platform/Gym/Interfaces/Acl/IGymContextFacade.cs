namespace SpotTrack.Platform.Gyms.Interfaces.Acl;

public interface IGymContextFacade
{
    Task<bool> OccupyEquipmentAsync(int equipmentId);
    Task<bool> ReleaseEquipmentAsync(int equipmentId);
    Task<bool> MarkEquipmentOutOfServiceAsync(int equipmentId);
}
