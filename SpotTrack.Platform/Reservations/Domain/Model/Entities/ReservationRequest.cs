namespace SpotTrack.Platform.Reservations.Domain.Model.Entities;

public class ReservationRequest
{
    private ReservationRequest() { }

    public ReservationRequest(int reservationId)
    {
        if (reservationId <= 0)
            throw new ArgumentOutOfRangeException(nameof(reservationId), reservationId,
                "ReservationId must be a positive integer.");

        ReservationId = reservationId;
        Status = EReservationRequestStatus.Submitted;
    }

    public int Id { get; private set; }
    public int ReservationId { get; private set; }
    public EReservationRequestStatus Status { get; private set; }

    public void ConfirmEquipmentOccupied()
    {
        if (Status is not EReservationRequestStatus.Submitted)
            throw new InvalidOperationException(
                $"Cannot confirm equipment occupied from '{Status}' status. Expected '{EReservationRequestStatus.Submitted}'.");

        Status = EReservationRequestStatus.EquipmentOccupied;
    }

    public void RequestEquipmentStatusChangeToAvailable()
    {
        if (Status is not EReservationRequestStatus.EquipmentOccupied)
            throw new InvalidOperationException(
                $"Cannot request equipment available from '{Status}' status. Expected '{EReservationRequestStatus.EquipmentOccupied}'.");

        Status = EReservationRequestStatus.EquipmentAvailableRequested;
    }

    public void RequestAlternativeEquipment()
    {
        if (Status is EReservationRequestStatus.AlternativeRequested
            or EReservationRequestStatus.EquipmentAvailableRequested)
            throw new InvalidOperationException(
                $"Cannot request alternative equipment from '{Status}' status.");

        Status = EReservationRequestStatus.AlternativeRequested;
    }
}
