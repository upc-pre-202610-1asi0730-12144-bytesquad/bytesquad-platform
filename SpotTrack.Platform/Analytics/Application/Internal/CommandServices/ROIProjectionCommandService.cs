using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Repositories;

namespace SpotTrack.Platform.Analytics.Application.Internal.CommandServices;

public class ROIProjectionCommandService : IROIProjectionCommandService
{
    private readonly IROIProjectionRepository _roiProjectionRepository;

    public ROIProjectionCommandService(IROIProjectionRepository roiProjectionRepository)
    {
        _roiProjectionRepository = roiProjectionRepository;
    }

    public async Task<ROIProjection?> Handle(RequestDowntimeCostProjectionCommand command)
    {
        var roiProjection = new ROIProjection(command);
        await _roiProjectionRepository.AddAsync(roiProjection);
        return roiProjection;
    }
    
    public async Task<ROIProjection?> Handle(RequestEarningsProjectionCommand command)
    {
        var roiProjectionId = new ROIProjectionId(command.RoiProjectionId);
        var roiProjection = await _roiProjectionRepository.FindByRoiProjectionIdAsync(roiProjectionId);
        
        if (roiProjection == null) return null;

        roiProjection.UpdateProjectedEarnings(command.ProjectedEarnings);
        await _roiProjectionRepository.UpdateAsync(roiProjection);
        return roiProjection;
    }

}