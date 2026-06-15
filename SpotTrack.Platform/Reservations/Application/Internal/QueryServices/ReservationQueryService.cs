using SpotTrack.Platform.Reservations.Application.QueryServices;
using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Domain.Model.Queries;
using SpotTrack.Platform.Reservations.Domain.Repositories;

namespace SpotTrack.Platform.Reservations.Application.Internal.QueryServices;


public class ReservationQueryService(IReservationRepository reservationRepository)
    : IReservationQueryService
{
   
    public async Task<Reservation?> Handle(
        GetReservationByIdQuery query,
        CancellationToken cancellationToken)
        => await reservationRepository.FindByIdAsync(query.ReservationId, cancellationToken);

    
    public async Task<IEnumerable<Reservation>> Handle(
        GetAllReservationsByClientIdQuery query,
        CancellationToken cancellationToken)
        => await reservationRepository.FindAllByClientIdAsync(query.ClientId, cancellationToken);


    public async Task<IEnumerable<Reservation>> Handle(
        GetAllReservationsByEquipmentIdQuery query,
        CancellationToken cancellationToken)
        => await reservationRepository.FindAllByEquipmentIdAsync(query.EquipmentId, cancellationToken);
}