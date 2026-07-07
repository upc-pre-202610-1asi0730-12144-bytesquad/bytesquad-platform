using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Iam.Interfaces.Acl;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Domain.Model.Events;
using SpotTrack.Platform.Memberships.Domain.Model.ValueObjects;
using SpotTrack.Platform.Profiles.Interfaces.Acl;
using SpotTrack.Platform.Shared.Application.Internal.EventHandlers;

namespace SpotTrack.Platform.Memberships.Application.Internal.EventHandlers;

public class PaymentConfirmedEventHandler(
    IIamContextFacade iamFacade,
    IProfilesContextFacade profilesFacade,
    IMembershipCommandService membershipCommandService,
    ILogger<PaymentConfirmedEventHandler> logger)
    : IEventHandler<PaymentConfirmedEvent>
{
    public async Task Handle(PaymentConfirmedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Purpose == PaymentPurpose.NewMembership)
        {
            await HandleMembershipRenewal(notification, cancellationToken);
            return;
        }

        if (notification.Purpose != PaymentPurpose.BusinessRegistration)
            return;

        var registrationId = notification.PendingRegistrationId!.Value;

        var userId = await iamFacade.ConsumePendingRegistrationAsync(registrationId, cancellationToken);
        if (userId == 0)
        {
            logger.LogError("PaymentConfirmed: failed to consume PendingRegistration {RegistrationId}", registrationId);
            return;
        }

        var data = await iamFacade.FetchPendingRegistrationAsync(registrationId, cancellationToken);
        if (data is null)
        {
            logger.LogError("PaymentConfirmed: PendingRegistration {RegistrationId} not found after consume", registrationId);
            return;
        }

        var adminId = await profilesFacade.CreateAdminAsync(userId, data.Email, data.FirstName,
            data.LastName, data.PhoneNumber, data.Dni);
        if (adminId == 0)
        {
            logger.LogError("PaymentConfirmed: failed to provision Admin profile for user {UserId}", userId);
            return;
        }

        var businessId = await profilesFacade.ProvisionBusinessAsync(adminId, data.CompanyName,
            data.Ruc, data.LegalStructure, data.CompanyPhone, data.CompanyEmail);
        if (businessId == 0)
        {
            logger.LogError("PaymentConfirmed: failed to provision Business for admin {AdminId}", adminId);
            return;
        }

        if (!Enum.TryParse<EMembershipPlan>(data.MembershipTier, ignoreCase: true, out var plan))
        {
            logger.LogError("PaymentConfirmed: unrecognised MembershipTier '{Tier}' for registration {RegistrationId}", data.MembershipTier, registrationId);
            return;
        }

        var now = DateTimeOffset.UtcNow;
        var membershipResult = await membershipCommandService.Handle(
            new CreateActivateMembershipCommand(userId, plan, now, now.AddDays(30)),
            cancellationToken);

        if (membershipResult.IsFailure)
            logger.LogError("PaymentConfirmed: failed to activate membership for user {UserId}: {Message}", userId, membershipResult.Message);
        else
            logger.LogInformation("PaymentConfirmed: provisioning complete for user {UserId}, membership {MembershipId}", userId, membershipResult.Value!.Id);
    }

    private async Task HandleMembershipRenewal(PaymentConfirmedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UserId is null)
        {
            logger.LogError("PaymentConfirmed(NewMembership): UserId is null for payment {PaymentId}", notification.PaymentId);
            return;
        }

        var userId = notification.UserId.Value;
        var now = DateTimeOffset.UtcNow;
        var result = await membershipCommandService.Handle(
            new CreateActivateMembershipCommand(userId, notification.MembershipPlan, now, now.AddDays(30)),
            cancellationToken);

        if (result.IsFailure)
            logger.LogError("PaymentConfirmed(NewMembership): failed to activate membership for user {UserId}: {Message}", userId, result.Message);
        else
            logger.LogInformation("PaymentConfirmed(NewMembership): membership {MembershipId} activated for user {UserId}", result.Value!.Id, userId);
    }
}
