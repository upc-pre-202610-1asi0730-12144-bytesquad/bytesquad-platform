namespace SpotTrack.Platform.Reservations.Domain.Model.Errors;

using SpotTrack.Platform.Shared.Domain.Model;

public static class ReservationsErrors
{
    public static Error ReservationNotFound(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.ReservationNotFound)}", message);

    public static Error InvalidReservationDates(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.InvalidReservationDates)}", message);

    public static Error EquipmentNotAvailable(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.EquipmentNotAvailable)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.InternalServerError)}", message);
    
    public static Error InvalidReservationStatus(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.InvalidReservationStatus)}", message);

    public static Error EquipmentOccupyFailed(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.EquipmentOccupyFailed)}", message);

    public static Error EquipmentReleaseFailed(string message) =>
        new($"{nameof(ReservationsError)}.{nameof(ReservationsError.EquipmentReleaseFailed)}", message);
}