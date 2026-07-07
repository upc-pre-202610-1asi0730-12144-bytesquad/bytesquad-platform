using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;
using SpotTrack.Platform.Analytics.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SpotTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MaintenanceQuoteRepository : IMaintenanceQuoteRepository
{
    private readonly AppDbContext _context;

    public MaintenanceQuoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MaintenanceQuote maintenanceQuote)
    {
        await _context.Set<MaintenanceQuote>().AddAsync(maintenanceQuote);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MaintenanceQuote maintenanceQuote)
    {
        _context.Set<MaintenanceQuote>().Update(maintenanceQuote);
        await _context.SaveChangesAsync();
    }

    public async Task<MaintenanceQuote?> FindByMaintenanceQuoteIdAsync(MaintenanceQuoteId maintenanceQuoteId)
    {
        return await _context.Set<MaintenanceQuote>()
            .FirstOrDefaultAsync(q => q.MaintenanceQuoteId.Value == maintenanceQuoteId.Value);
    }

    public async Task<IEnumerable<MaintenanceQuote>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<MaintenanceQuote>()
            .Where(q => q.AdminId == adminId)
            .ToListAsync(cancellationToken);
    }
}
