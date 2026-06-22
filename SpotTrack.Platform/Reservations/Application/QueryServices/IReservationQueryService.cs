using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Domain.Model.Queries;

namespace SpotTrack.Platform.Reservations.Application.QueryServices;


public interface IReservationQueryService
{
  
    Task<Reservation?> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken);

   
    Task<IEnumerable<Reservation>> Handle(GetAllReservationsByClientIdQuery query, CancellationToken cancellationToken);

    
    Task<IEnumerable<Reservation>> Handle(GetAllReservationsByEquipmentIdQuery query, CancellationToken cancellationToken);
}