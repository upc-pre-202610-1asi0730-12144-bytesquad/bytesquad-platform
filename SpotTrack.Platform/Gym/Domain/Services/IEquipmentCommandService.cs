using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Gyms.Domain.Services;

public interface IEquipmentCommandService
{
    Task<Result<Equipment>> Handle(RegisterEquipmentCommand command, CancellationToken cancellationToken);
    Task<Result<Equipment>> Handle(OccupyEquipmentCommand command, CancellationToken cancellationToken);
    Task<Result<Equipment>> Handle(ReleaseEquipmentCommand command, CancellationToken cancellationToken);
    Task<Result<Equipment>> Handle(MarkEquipmentOutOfServiceCommand command, CancellationToken cancellationToken);
}
