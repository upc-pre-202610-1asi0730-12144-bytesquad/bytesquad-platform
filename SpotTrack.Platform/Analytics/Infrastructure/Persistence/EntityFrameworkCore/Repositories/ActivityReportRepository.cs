using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;
using SpotTrack.Platform.Analytics.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SpotTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ActivityReportRepository : IActivityReportRepository
{
    private readonly AppDbContext _context;

    public ActivityReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ActivityReport activityReport)
    {
        await _context.Set<ActivityReport>().AddAsync(activityReport);
        await _context.SaveChangesAsync();
    }

    public async Task<ActivityReport?> FindByActivityReportIdAsync(ActivityReportId activityReportId)
    {
        return await _context.Set<ActivityReport>()
            .FirstOrDefaultAsync(r => r.ActivityReportId.Value == activityReportId.Value);
    }

    public async Task UpdateAsync(ActivityReport activityReport)
    {
        _context.Set<ActivityReport>().Update(activityReport);
        await _context.SaveChangesAsync();
    }
}
