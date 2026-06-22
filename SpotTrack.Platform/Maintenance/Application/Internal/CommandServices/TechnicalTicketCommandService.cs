using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Gyms.Interfaces.Acl;
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

public class TechnicalTicketCommandService(
    ITechnicalTicketRepository technicalTicketRepository,
    IMaintenanceRepository maintenanceRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<MaintenanceMessages> localizer,
    IGymContextFacade gymContextFacade)
    : ITechnicalTicketCommandService
{
    public async Task<Result<TechnicalTicket>> Handle(
        CreateTechnicalTicketCommand command,
        CancellationToken cancellationToken)
    {
        var maintenance = await maintenanceRepository.FindByIdAsync(command.MaintenanceId, cancellationToken);
        if (maintenance is null)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.MaintenanceNotFound,
                localizer[nameof(TechnicalTicketError.MaintenanceNotFound)]);

        TechnicalTicket ticket;
        try
        {
            ticket = new TechnicalTicket(command);
        }
        catch (ArgumentException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InternalServerError,
                localizer[nameof(TechnicalTicketError.InternalServerError)]);
        }

        try
        {
            await technicalTicketRepository.AddAsync(ticket, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.OperationCancelled,
                localizer[nameof(TechnicalTicketError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.DatabaseError,
                localizer[nameof(TechnicalTicketError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InternalServerError,
                localizer[nameof(TechnicalTicketError.InternalServerError)]);
        }

        var marked = await gymContextFacade.MarkEquipmentOutOfServiceAsync(command.EquipmentId);
        if (!marked)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.EquipmentUpdateFailed,
                localizer[nameof(TechnicalTicketError.EquipmentUpdateFailed)]);

        await mediator.PublishAsync(
            TechnicalTicketCreatedEvent.FromTechnicalTicket(ticket),
            cancellationToken);

        return Result<TechnicalTicket>.Success(ticket);
    }

    public async Task<Result<TechnicalTicket>> Handle(
        AssignTechnicalTicketCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await technicalTicketRepository.FindByIdAsync(command.TechnicalTicketId, cancellationToken);
        if (ticket is null)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.TechnicalTicketNotFound,
                localizer[nameof(TechnicalTicketError.TechnicalTicketNotFound)]);

        try
        {
            ticket.Assign(command.TechnicianId);
        }
        catch (InvalidOperationException ex)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InvalidTechnicalTicketStatus,
                ex.Message);
        }

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                TechnicalTicketAssignedEvent.FromTechnicalTicket(ticket),
                cancellationToken);

            return Result<TechnicalTicket>.Success(ticket);
        }
        catch (DbUpdateException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.DatabaseError,
                localizer[nameof(TechnicalTicketError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InternalServerError,
                localizer[nameof(TechnicalTicketError.InternalServerError)]);
        }
    }

    public async Task<Result<TechnicalTicket>> Handle(
        ModifyTicketStatusCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await technicalTicketRepository.FindByIdAsync(command.TechnicalTicketId, cancellationToken);
        if (ticket is null)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.TechnicalTicketNotFound,
                localizer[nameof(TechnicalTicketError.TechnicalTicketNotFound)]);

        try
        {
            ticket.ModifyStatus(command.NewStatus);
        }
        catch (InvalidOperationException ex)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InvalidTechnicalTicketStatus,
                ex.Message);
        }

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                TicketStatusModifiedEvent.FromTechnicalTicket(ticket),
                cancellationToken);

            return Result<TechnicalTicket>.Success(ticket);
        }
        catch (DbUpdateException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.DatabaseError,
                localizer[nameof(TechnicalTicketError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InternalServerError,
                localizer[nameof(TechnicalTicketError.InternalServerError)]);
        }
    }

    public async Task<Result<TechnicalTicket>> Handle(
        RequestUpdateMaintenanceStatusCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await technicalTicketRepository.FindByIdAsync(command.TechnicalTicketId, cancellationToken);
        if (ticket is null)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.TechnicalTicketNotFound,
                localizer[nameof(TechnicalTicketError.TechnicalTicketNotFound)]);

        try
        {
            ticket.RequestMaintenanceStatusUpdate();
        }
        catch (InvalidOperationException ex)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InvalidTechnicalTicketStatus,
                ex.Message);
        }

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                MaintenanceStatusUpdateRequestedEvent.FromTechnicalTicket(ticket),
                cancellationToken);

            return Result<TechnicalTicket>.Success(ticket);
        }
        catch (DbUpdateException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.DatabaseError,
                localizer[nameof(TechnicalTicketError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InternalServerError,
                localizer[nameof(TechnicalTicketError.InternalServerError)]);
        }
    }

    public async Task<Result<TechnicalTicket>> Handle(
        UpdateMaintenanceStatusCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await technicalTicketRepository.FindByIdAsync(command.TechnicalTicketId, cancellationToken);
        if (ticket is null)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.TechnicalTicketNotFound,
                localizer[nameof(TechnicalTicketError.TechnicalTicketNotFound)]);

        try
        {
            ticket.UpdateMaintenanceProgress(command.NewProgress);
        }
        catch (InvalidOperationException ex)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InvalidTechnicalTicketStatus,
                ex.Message);
        }

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                MaintenanceStatusUpdatedEvent.FromTechnicalTicket(ticket),
                cancellationToken);

            return Result<TechnicalTicket>.Success(ticket);
        }
        catch (DbUpdateException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.DatabaseError,
                localizer[nameof(TechnicalTicketError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InternalServerError,
                localizer[nameof(TechnicalTicketError.InternalServerError)]);
        }
    }

    public async Task<Result<TechnicalTicket>> Handle(
        CompleteMaintenanceCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await technicalTicketRepository.FindByIdAsync(command.TechnicalTicketId, cancellationToken);
        if (ticket is null)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.TechnicalTicketNotFound,
                localizer[nameof(TechnicalTicketError.TechnicalTicketNotFound)]);

        try
        {
            ticket.Complete();
        }
        catch (InvalidOperationException ex)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InvalidTechnicalTicketStatus,
                ex.Message);
        }

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.DatabaseError,
                localizer[nameof(TechnicalTicketError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.InternalServerError,
                localizer[nameof(TechnicalTicketError.InternalServerError)]);
        }

        var marked = await gymContextFacade.MarkEquipmentAvailableAsync(ticket.EquipmentId);
        if (!marked)
            return Result<TechnicalTicket>.Failure(
                TechnicalTicketError.EquipmentUpdateFailed,
                localizer[nameof(TechnicalTicketError.EquipmentUpdateFailed)]);

        await mediator.PublishAsync(
            TicketStatusMarkedAsResolvedEvent.FromTechnicalTicket(ticket),
            cancellationToken);

        return Result<TechnicalTicket>.Success(ticket);
    }
}
