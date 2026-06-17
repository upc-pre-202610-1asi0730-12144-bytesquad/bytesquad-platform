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
    public EMembershipStatus Status { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    public MembershipPeriod Period => new(StartDate, EndDate);

    // Future features: UpgradePlan, Suspend, Renew, Cancel
}
