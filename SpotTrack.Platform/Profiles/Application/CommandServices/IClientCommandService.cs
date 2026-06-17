using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Profiles.Application.CommandServices;

public interface IClientCommandService
{
    Task<Result<Client>> Handle(CreateClientCommand command, CancellationToken cancellationToken);
    Task<Result<Client>> Handle(RegisterClientCommand command, CancellationToken cancellationToken);
    Task<Result<Client>> Handle(UpdateClientProfileCommand command, CancellationToken cancellationToken);
}
