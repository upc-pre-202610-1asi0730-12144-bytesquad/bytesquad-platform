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

public class MaintenanceJobCommandService(
    IMaintenanceJobRepository maintenanceJobRepository,
    ITechnicalTicketRepository technicalTicketRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<MaintenanceMessages> localizer)
    : IMaintenanceJobCommandService
{
    public async Task<Result<MaintenanceJob>> Handle(
        CreateAcceptMaintenanceCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await technicalTicketRepository.FindByIdAsync(command.TechnicalTicketId, cancellationToken);
        if (ticket is null)
            return Result<MaintenanceJob>.Failure(
                MaintenanceJobError.TechnicalTicketNotFound,
                localizer[nameof(MaintenanceJobError.TechnicalTicketNotFound)]);

        MaintenanceJob job;
        try
        {
            job = new MaintenanceJob(command);
        }
        catch (ArgumentException)
        {
            return Result<MaintenanceJob>.Failure(
                MaintenanceJobError.InternalServerError,
                localizer[nameof(MaintenanceJobError.InternalServerError)]);
        }

        try
        {
            await maintenanceJobRepository.AddAsync(job, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                MaintenanceJobAcceptedEvent.FromMaintenanceJob(job),
                cancellationToken);

            return Result<MaintenanceJob>.Success(job);
        }
        catch (OperationCanceledException)
        {
            return Result<MaintenanceJob>.Failure(
                MaintenanceJobError.OperationCancelled,
                localizer[nameof(MaintenanceJobError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<MaintenanceJob>.Failure(
                MaintenanceJobError.DatabaseError,
                localizer[nameof(MaintenanceJobError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<MaintenanceJob>.Failure(
                MaintenanceJobError.InternalServerError,
                localizer[nameof(MaintenanceJobError.InternalServerError)]);
        }
    }
}
