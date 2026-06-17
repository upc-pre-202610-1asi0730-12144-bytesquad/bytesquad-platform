using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Domain.Model.Events;
using SpotTrack.Platform.Memberships.Domain.Repositories;
using SpotTrack.Platform.Memberships.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Memberships.Application.Internal.CommandServices;

public class BranchAccessCommandService(
    IBranchAccessRepository branchAccessRepository,
    IMembershipRepository membershipRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<MembershipMessages> localizer)
    : IBranchAccessCommandService
{
    public async Task<Result<BranchAccess>> Handle(
        CreateGrantBranchAccessCommand command,
        CancellationToken cancellationToken)
    {
        var membership = await membershipRepository.FindByIdAsync(command.MembershipId, cancellationToken);

        var isActive = membership is { Status: EMembershipStatus.Active };

        var branchAccess = isActive
            ? BranchAccess.Grant(command.MembershipId, command.BranchId, command.GrantedByAdminId)
            : BranchAccess.Deny(command.MembershipId, command.BranchId, command.GrantedByAdminId);

        try
        {
            await branchAccessRepository.AddAsync(branchAccess, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            if (isActive)
                await mediator.PublishAsync(
                    BranchAccessGrantedEvent.FromBranchAccess(branchAccess),
                    cancellationToken);
            else
                await mediator.PublishAsync(
                    BranchAccessDeniedEvent.FromBranchAccess(branchAccess),
                    cancellationToken);

            return Result<BranchAccess>.Success(branchAccess);
        }
        catch (OperationCanceledException)
        {
            return Result<BranchAccess>.Failure(
                BranchAccessError.OperationCancelled,
                localizer[nameof(BranchAccessError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<BranchAccess>.Failure(
                BranchAccessError.DatabaseError,
                localizer[nameof(BranchAccessError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<BranchAccess>.Failure(
                BranchAccessError.InternalServerError,
                localizer[nameof(BranchAccessError.InternalServerError)]);
        }
    }
}
