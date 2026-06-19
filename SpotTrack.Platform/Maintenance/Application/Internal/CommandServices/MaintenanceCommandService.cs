using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Maintenances.Application.CommandServices;
using SpotTrack.Platform.Maintenances.Domain.Model;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Domain.Model.Events;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Maintenances.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Application.Internal.CommandServices;

public class MaintenanceCommandService(
    IMaintenanceRepository maintenanceRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<MaintenanceMessages> localizer)
    : IMaintenanceCommandService
{
    public async Task<Result<Maintenance>> Handle(
        CreateRequestMaintenanceCommand command,
        CancellationToken cancellationToken)
    {
        Maintenance maintenance;

        try
        {
            maintenance = new Maintenance(command);
        }
        catch (ArgumentException)
        {
            return Result<Maintenance>.Failure(
                MaintenanceError.InvalidMaintenanceData,
                localizer[nameof(MaintenanceError.InvalidMaintenanceData)]);
        }

        try
        {
            await maintenanceRepository.AddAsync(maintenance, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                MaintenanceRequestedEvent.FromMaintenance(maintenance),
                cancellationToken);

            return Result<Maintenance>.Success(maintenance);
        }
        catch (OperationCanceledException)
        {
            return Result<Maintenance>.Failure(
                MaintenanceError.OperationCancelled,
                localizer[nameof(MaintenanceError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Maintenance>.Failure(
                MaintenanceError.DatabaseError,
                localizer[nameof(MaintenanceError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Maintenance>.Failure(
                MaintenanceError.InternalServerError,
                localizer[nameof(MaintenanceError.InternalServerError)]);
        }
    }
}
