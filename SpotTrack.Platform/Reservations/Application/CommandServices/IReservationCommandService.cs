using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Reservations.Application.CommandServices;


public interface IReservationCommandService
{
  
    Task<Result<Reservation>> Handle(CreateInitiateExpressReservationCommand command, CancellationToken cancellationToken);
    Task<Result<Reservation>>Handle(CreateCancelReservationCommand command, CancellationToken cancellationToken);
    Task<Result<Reservation>>Handle(CreateSubmitRequestOccupyEquipmentCommand command, CancellationToken cancellationToken);
    Task<Result<Reservation>>Handle(CreateEndReservationCommand command, CancellationToken cancellationToken);
    Task<Result<Reservation>> Handle(CreateStartReservationTimerCommand command, CancellationToken cancellationToken);
    Task<Result<Reservation>> Handle(CreateRequestEquipmentStatusChangeToAvailableCommand command, CancellationToken cancellationToken);
}
