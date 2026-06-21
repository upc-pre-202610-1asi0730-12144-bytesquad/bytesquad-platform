using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Repositories;

public interface IMaintenanceQuoteRepository
{
    Task AddAsync(MaintenanceQuote maintenanceQuote);
    Task UpdateAsync(MaintenanceQuote maintenanceQuote);
    Task<MaintenanceQuote?> FindByMaintenanceQuoteIdAsync(MaintenanceQuoteId maintenanceQuoteId);
}