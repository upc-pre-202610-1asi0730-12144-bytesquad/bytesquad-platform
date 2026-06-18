using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Gyms.Domain.Model.Errors;

public static class EquipmentErrors
{
    public static Error ZoneNotFound(string message) =>
        new($"{nameof(EquipmentError)}.{nameof(EquipmentError.ZoneNotFound)}", message);

    public static Error InvalidData(string message) =>
        new($"{nameof(EquipmentError)}.{nameof(EquipmentError.InvalidData)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(EquipmentError)}.{nameof(EquipmentError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(EquipmentError)}.{nameof(EquipmentError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(EquipmentError)}.{nameof(EquipmentError.InternalServerError)}", message);
}
