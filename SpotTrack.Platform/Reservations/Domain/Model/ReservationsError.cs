namespace SpotTrack.Platform.Reservations.Domain.Model;

public enum ReservationsError
{
    ReservationNotFound,
    InvalidReservationDates,
    EquipmentNotAvailable,
    OperationCancelled,
    DatabaseError,
    InternalServerError,
    InvalidReservationStatus,
    EquipmentOccupyFailed,
    EquipmentReleaseFailed,
}