namespace SpotTrack.Platform.Gyms.Interfaces.Acl;

public interface IGymContextFacade
{
    Task<bool> OccupyEquipmentAsync(int equipmentId);
    Task<bool> ReleaseEquipmentAsync(int equipmentId);
    Task<bool> MarkEquipmentOutOfServiceAsync(int equipmentId);
    Task<bool> MarkEquipmentAvailableAsync(int equipmentId);
    Task<int?> GetAdminIdByEquipmentIdAsync(int equipmentId, CancellationToken cancellationToken);
    Task<IEnumerable<int>> GetEquipmentIdsByAdminIdAsync(int adminId, CancellationToken cancellationToken = default);
    Task<int> CreateGymAsync(int adminId, string name, string street, string district, string city,
        CancellationToken cancellationToken = default);
}
