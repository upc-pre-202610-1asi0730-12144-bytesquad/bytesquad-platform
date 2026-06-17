using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Profiles.Application.CommandServices;

public interface IAdminCommandService
{
    Task<Result<Admin>> Handle(CreateAdminCommand command, CancellationToken cancellationToken);
    Task<Result<Admin>> Handle(RegisterAdminCommand command, CancellationToken cancellationToken);
    Task<Result<Admin>> Handle(UpdateAdminProfileCommand command, CancellationToken cancellationToken);
}
