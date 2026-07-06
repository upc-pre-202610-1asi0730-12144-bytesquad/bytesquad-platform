using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Gyms.Interfaces.Acl;

public interface IGymContextFacade
{
    Task<bool> OccupyEquipmentAsync(int equipmentId);
    Task<bool> ReleaseEquipmentAsync(int equipmentId);
    Task<bool> MarkEquipmentOutOfServiceAsync(int equipmentId);
    Task<bool> MarkEquipmentAvailableAsync(int equipmentId);
    Task<Equipment?> FindEquipmentByIdAsync(int equipmentId);
    Task<IEnumerable<Equipment>> FindAvailableAlternativesAsync(string equipmentName, int excludeEquipmentId);
}
