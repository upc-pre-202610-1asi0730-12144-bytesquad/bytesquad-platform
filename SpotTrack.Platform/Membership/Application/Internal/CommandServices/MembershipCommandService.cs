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

public class MembershipCommandService(
    IMembershipRepository membershipRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<MembershipMessages> localizer)
    : IMembershipCommandService
{
    public async Task<Result<Membership>> Handle(
        CreateActivateMembershipCommand command,
        CancellationToken cancellationToken)
    {
        Membership membership;

        try
        {
            membership = new Membership(command);
        }
        catch (ArgumentException)
        {
            return Result<Membership>.Failure(
                MembershipError.InvalidMembershipPeriod,
                localizer[nameof(MembershipError.InvalidMembershipPeriod)]);
        }

        try
        {
            await membershipRepository.AddAsync(membership, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                GymMembershipActivatedEvent.FromMembership(membership),
                cancellationToken);

            return Result<Membership>.Success(membership);
        }
        catch (OperationCanceledException)
        {
            return Result<Membership>.Failure(
                MembershipError.OperationCancelled,
                localizer[nameof(MembershipError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Membership>.Failure(
                MembershipError.DatabaseError,
                localizer[nameof(MembershipError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Membership>.Failure(
                MembershipError.InternalServerError,
                localizer[nameof(MembershipError.InternalServerError)]);
        }
    }

    public async Task<Result<Membership>> Handle(
        CreateUpgradeMembershipPlanCommand command,
        CancellationToken cancellationToken)
    {
        var membership = await membershipRepository.FindByIdAsync(command.MembershipId, cancellationToken);

        if (membership is null)
            return Result<Membership>.Failure(
                MembershipError.MembershipNotFound,
                localizer[nameof(MembershipError.MembershipNotFound)]);

        try
        {
            membership.UpgradePlan(command.NewPlan);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Active"))
        {
            return Result<Membership>.Failure(
                MembershipError.InvalidMembershipStatus,
                localizer[nameof(MembershipError.InvalidMembershipStatus)]);
        }
        catch (InvalidOperationException)
        {
            return Result<Membership>.Failure(
                MembershipError.InvalidMembershipPlan,
                localizer[nameof(MembershipError.InvalidMembershipPlan)]);
        }

        try
        {
            membershipRepository.Update(membership);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                PlanUpgradedEvent.FromMembership(membership),
                cancellationToken);

            return Result<Membership>.Success(membership);
        }
        catch (OperationCanceledException)
        {
            return Result<Membership>.Failure(
                MembershipError.OperationCancelled,
                localizer[nameof(MembershipError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Membership>.Failure(
                MembershipError.DatabaseError,
                localizer[nameof(MembershipError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Membership>.Failure(
                MembershipError.InternalServerError,
                localizer[nameof(MembershipError.InternalServerError)]);
        }
    }

    public async Task<Result<Membership>> Handle(
        CreateSuspendMembershipCommand command,
        CancellationToken cancellationToken)
    {
        var membership = await membershipRepository.FindByIdAsync(command.MembershipId, cancellationToken);

        if (membership is null)
            return Result<Membership>.Failure(
                MembershipError.MembershipNotFound,
                localizer[nameof(MembershipError.MembershipNotFound)]);

        try
        {
            membership.Suspend();
        }
        catch (InvalidOperationException)
        {
            return Result<Membership>.Failure(
                MembershipError.InvalidMembershipStatus,
                localizer[nameof(MembershipError.InvalidMembershipStatus)]);
        }

        try
        {
            membershipRepository.Update(membership);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                MembershipSuspendedEvent.FromMembership(membership),
                cancellationToken);

            return Result<Membership>.Success(membership);
        }
        catch (OperationCanceledException)
        {
            return Result<Membership>.Failure(
                MembershipError.OperationCancelled,
                localizer[nameof(MembershipError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Membership>.Failure(
                MembershipError.DatabaseError,
                localizer[nameof(MembershipError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Membership>.Failure(
                MembershipError.InternalServerError,
                localizer[nameof(MembershipError.InternalServerError)]);
        }
    }

    public async Task<Result<Membership>> Handle(
        CreateRenewMembershipCommand command,
        CancellationToken cancellationToken)
    {
        var membership = await membershipRepository.FindByIdAsync(command.MembershipId, cancellationToken);

        if (membership is null)
            return Result<Membership>.Failure(
                MembershipError.MembershipNotFound,
                localizer[nameof(MembershipError.MembershipNotFound)]);

        try
        {
            membership.Renew(command.NewEndDate);
        }
        catch (InvalidOperationException)
        {
            return Result<Membership>.Failure(
                MembershipError.InvalidMembershipStatus,
                localizer[nameof(MembershipError.InvalidMembershipStatus)]);
        }
        catch (ArgumentOutOfRangeException)
        {
            return Result<Membership>.Failure(
                MembershipError.InvalidMembershipPeriod,
                localizer[nameof(MembershipError.InvalidMembershipPeriod)]);
        }

        try
        {
            membershipRepository.Update(membership);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                GymMembershipRenewedEvent.FromMembership(membership),
                cancellationToken);

            return Result<Membership>.Success(membership);
        }
        catch (OperationCanceledException)
        {
            return Result<Membership>.Failure(
                MembershipError.OperationCancelled,
                localizer[nameof(MembershipError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Membership>.Failure(
                MembershipError.DatabaseError,
                localizer[nameof(MembershipError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Membership>.Failure(
                MembershipError.InternalServerError,
                localizer[nameof(MembershipError.InternalServerError)]);
        }
    }
}
