namespace SpotTrack.Platform.Memberships.Domain.Model.ValueObjects;

public record MembershipPeriod
{
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }

    public MembershipPeriod(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException(
                "EndDate must be strictly after StartDate.", nameof(endDate));

        StartDate = startDate;
        EndDate = endDate;
    }

    public string Display => $"{StartDate:O} / {EndDate:O}";
}
