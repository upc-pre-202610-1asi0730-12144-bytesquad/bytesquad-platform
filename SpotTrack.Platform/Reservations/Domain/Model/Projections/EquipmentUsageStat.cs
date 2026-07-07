namespace SpotTrack.Platform.Reservations.Domain.Model.Projections;

public record EquipmentUsageStat(int EquipmentId, double TotalUsageHours, int ReservationCount);
