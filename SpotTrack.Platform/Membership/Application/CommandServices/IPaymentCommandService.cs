using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Memberships.Application.CommandServices;

public interface IPaymentCommandService
{
    Task<Result<string>> Handle(InitiateBusinessPaymentCommand command, CancellationToken cancellationToken);
    Task<Result<string>> Handle(InitiateMembershipPaymentCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(FailPaymentCommand command, CancellationToken cancellationToken);
}
