namespace SpotTrack.Platform.Memberships.Domain.Model;

public static class MembershipPlanExtensions
{
    public static (decimal Amount, string Currency) ToPrice(this EMembershipPlan plan) => plan switch
    {
        EMembershipPlan.Basic   => (69m,  "usd"),
        EMembershipPlan.Mid     => (109m, "usd"),
        EMembershipPlan.Premium => (189m, "usd"),
        _ => throw new ArgumentOutOfRangeException(nameof(plan), plan, "No price defined for plan.")
    };

    public static long ToStripeAmount(this EMembershipPlan plan)
    {
        var (amount, _) = plan.ToPrice();
        return (long)(amount * 100);
    }
}
