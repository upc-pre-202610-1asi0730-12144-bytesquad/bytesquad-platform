namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

public record EquipmentUsageStatResource(int EquipmentId, double TotalUsageHours, int ReservationCount);
