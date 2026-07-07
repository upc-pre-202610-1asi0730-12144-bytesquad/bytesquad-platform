namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

public record HourlyUsageStatResource(int Hour, int ReservationCount, double TotalMinutes);
