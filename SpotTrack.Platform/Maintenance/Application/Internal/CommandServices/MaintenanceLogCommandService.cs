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

public class MaintenanceLogCommandService(
    IMaintenanceLogRepository maintenanceLogRepository,
    ITechnicalTicketRepository technicalTicketRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<MaintenanceMessages> localizer)
    : IMaintenanceLogCommandService
{
    public async Task<Result<MaintenanceLog>> Handle(
        RegisterMaintenanceCompletionCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await technicalTicketRepository.FindByIdAsync(command.TechnicalTicketId, cancellationToken);
        if (ticket is null)
            return Result<MaintenanceLog>.Failure(
                MaintenanceLogError.TechnicalTicketNotFound,
                localizer[nameof(MaintenanceLogError.TechnicalTicketNotFound)]);

        if (ticket.Status is not ETechnicalTicketStatus.Resolved)
            return Result<MaintenanceLog>.Failure(
                MaintenanceLogError.TicketNotResolved,
                localizer[nameof(MaintenanceLogError.TicketNotResolved)]);

        var log = new MaintenanceLog(command, ticket.EquipmentId);

        try
        {
            await maintenanceLogRepository.AddAsync(log, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                MaintenanceCompletionRegisteredEvent.FromMaintenanceLog(log),
                cancellationToken);

            return Result<MaintenanceLog>.Success(log);
        }
        catch (OperationCanceledException)
        {
            return Result<MaintenanceLog>.Failure(
                MaintenanceLogError.OperationCancelled,
                localizer[nameof(MaintenanceLogError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<MaintenanceLog>.Failure(
                MaintenanceLogError.DatabaseError,
                localizer[nameof(MaintenanceLogError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<MaintenanceLog>.Failure(
                MaintenanceLogError.InternalServerError,
                localizer[nameof(MaintenanceLogError.InternalServerError)]);
        }
    }
}
