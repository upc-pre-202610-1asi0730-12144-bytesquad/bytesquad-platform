using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Memberships.Domain.Model.Aggregates;

public partial class Membership
{
    private Membership() { }

    public Membership(CreateActivateMembershipCommand command)
    {
        if (command.ClientId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.ClientId), command.ClientId,
                "ClientId must be a positive integer.");

        var period = new MembershipPeriod(command.StartDate, command.EndDate);

        ClientId = command.ClientId;
        Plan = command.Plan;
        StartDate = period.StartDate;
        EndDate = period.EndDate;
        Status = EMembershipStatus.Active;
    }

    public int Id { get; private set; }
    public int ClientId { get; private set; }
    public EMembershipPlan Plan { get; private set; }
    public EMembershipPlan? PendingDowngradePlan { get; private set; }
    public EMembershipStatus Status { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    public MembershipPeriod Period => new(StartDate, EndDate);

    public void Renew(DateTimeOffset newEndDate)
    {
        if (Status == EMembershipStatus.Cancelled)
            throw new InvalidOperationException("A cancelled membership cannot be renewed.");

        if (newEndDate <= EndDate)
            throw new ArgumentOutOfRangeException(nameof(newEndDate), "New end date must be after the current end date.");

        EndDate = newEndDate;
        Status = EMembershipStatus.Active;
    }

    public void UpgradePlan(EMembershipPlan newPlan)
    {
        if (Status != EMembershipStatus.Active)
            throw new InvalidOperationException("Membership must be Active to upgrade the plan.");

        if (newPlan <= Plan)
            throw new InvalidOperationException("New plan must be superior to the current plan.");

        Plan = newPlan;
        PendingDowngradePlan = null;
    }

    public void Suspend()
    {
        if (Status == EMembershipStatus.Cancelled ||
            Status == EMembershipStatus.Suspended ||
            Status == EMembershipStatus.Expired)
            throw new InvalidOperationException("Only an Active membership can be suspended.");

        Status = EMembershipStatus.Suspended;
    }

    public void RequestCancellation()
    {
        if (Status is EMembershipStatus.PendingCancellation
                    or EMembershipStatus.Cancelled
                    or EMembershipStatus.Expired)
            throw new InvalidOperationException(
                $"Cannot request cancellation of a membership in '{Status}' status.");

        Status = EMembershipStatus.PendingCancellation;
    }

    public void CompleteCancellation()
    {
        if (Status != EMembershipStatus.PendingCancellation)
            throw new InvalidOperationException(
                "Only a PendingCancellation membership can complete its cancellation.");

        Status = EMembershipStatus.Cancelled;
    }

    public void Expire()
    {
        if (Status != EMembershipStatus.Active)
            throw new InvalidOperationException("Only an Active membership can expire.");

        if (PendingDowngradePlan.HasValue)
        {
            Plan = PendingDowngradePlan.Value;
            PendingDowngradePlan = null;
        }

        Status = EMembershipStatus.Expired;
    }

    public void RequestDowngrade(EMembershipPlan newPlan)
    {
        if (Status != EMembershipStatus.Active)
            throw new InvalidOperationException("Membership must be Active to request a plan downgrade.");

        if (newPlan >= Plan)
            throw new InvalidOperationException("New plan must be inferior to the current plan.");

        PendingDowngradePlan = newPlan;
    }
}
