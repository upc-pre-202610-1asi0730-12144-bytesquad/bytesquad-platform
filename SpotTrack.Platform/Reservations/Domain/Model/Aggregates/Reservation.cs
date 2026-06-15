using SpotTrack.Platform.Reservations.Domain.Model.Commands;
using SpotTrack.Platform.Reservations.Domain.Model.Entities;
using SpotTrack.Platform.Reservations.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Reservations.Domain.Model.Aggregates;

public partial class Reservation
{
    private Reservation() { }

    public Reservation(CreateInitiateExpressReservationCommand command)
    {
        if (command.ClientId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.ClientId), command.ClientId,
                "ClientId must be a positive integer.");

        if (command.EquipmentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.EquipmentId), command.EquipmentId,
                "EquipmentId must be a positive integer.");

        ClientId = command.ClientId;
        EquipmentId = command.EquipmentId;

        var period = new ReservationPeriod(command.StartDate, command.EndDate);

        StartDate = period.StartDate;
        EndDate = period.EndDate;

        Status = EReservationStatus.Initiated;
    }
    

    public int Id { get; private set; }
    public int ClientId { get; private set; }
    public int EquipmentId { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    public ReservationPeriod Period => new(StartDate, EndDate);

    public EReservationStatus Status { get; private set; }

    private ReservationRequest? _request;
    public ReservationRequest? Request => _request;

    public void SubmitRequest()
    {
        if (Status is not EReservationStatus.Initiated)
            throw new InvalidOperationException(
                $"Cannot submit a request for a reservation in '{Status}' status.");

        if (_request is not null)
            throw new InvalidOperationException(
                "A request has already been submitted for this reservation.");

        _request = new ReservationRequest(Id);
        Status = EReservationStatus.Reserved;
    }

    public void Cancel()
    {
        if (Status is EReservationStatus.Ended or EReservationStatus.Cancelled)
            throw new InvalidOperationException(
                $"Cannot cancel a reservation that is already in '{Status}' status.");

        Status = EReservationStatus.Cancelled;
    }

    public void End()
    {
        if (Status is EReservationStatus.Ended or EReservationStatus.Cancelled)
            throw new InvalidOperationException(
                $"Cannot end a reservation that is already in '{Status}' status.");

        Status = EReservationStatus.Ended;
    }
}
