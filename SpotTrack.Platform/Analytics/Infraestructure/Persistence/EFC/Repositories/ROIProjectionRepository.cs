using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;
using SpotTrack.Platform.Analytics.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace SpotTrack.Platform.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class ROIProjectionRepository : IROIProjectionRepository
{
    private readonly AppDbContext _context;

    public ROIProjectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ROIProjection roiProjection)
    {
        await _context.Set<ROIProjection>().AddAsync(roiProjection);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ROIProjection roiProjection)
    {
        _context.Set<ROIProjection>().Update(roiProjection);
        await _context.SaveChangesAsync();
    }

    public async Task<ROIProjection?> FindByRoiProjectionIdAsync(ROIProjectionId roiProjectionId)
    {
        return await _context.Set<ROIProjection>()
            .FirstOrDefaultAsync(p => p.RoiProjectionId == roiProjectionId);
    }
}