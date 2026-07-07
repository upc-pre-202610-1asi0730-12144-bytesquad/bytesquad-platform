namespace SpotTrack.Platform.Reservations.Domain.Model.Projections;

public record HourlyUsageStat(int Hour, int ReservationCount, double TotalMinutes);
