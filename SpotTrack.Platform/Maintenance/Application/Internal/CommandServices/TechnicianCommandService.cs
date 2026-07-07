using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Maintenances.Application.CommandServices;
using SpotTrack.Platform.Maintenances.Domain.Model;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Maintenances.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Application.Internal.CommandServices;

public class TechnicianCommandService(
    ITechnicianRepository technicianRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<MaintenanceMessages> localizer)
    : ITechnicianCommandService
{
    public async Task<Result<Technician>> Handle(
        CreateTechnicianCommand command,
        CancellationToken cancellationToken = default)
    {
        Technician technician;
        try
        {
            technician = new Technician(command);
        }
        catch (ArgumentException)
        {
            return Result<Technician>.Failure(
                TechnicianError.InvalidTechnicianData,
                localizer[nameof(TechnicianError.InvalidTechnicianData)]);
        }

        try
        {
            await technicianRepository.AddAsync(technician, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Technician>.Success(technician);
        }
        catch (OperationCanceledException)
        {
            return Result<Technician>.Failure(
                TechnicianError.OperationCancelled,
                localizer[nameof(TechnicianError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Technician>.Failure(
                TechnicianError.DatabaseError,
                localizer[nameof(TechnicianError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Technician>.Failure(
                TechnicianError.InternalServerError,
                localizer[nameof(TechnicianError.InternalServerError)]);
        }
    }
}
