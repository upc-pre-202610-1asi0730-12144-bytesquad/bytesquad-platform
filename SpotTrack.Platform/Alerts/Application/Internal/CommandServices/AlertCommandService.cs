using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Alerts.Application.CommandServices;
using SpotTrack.Platform.Alerts.Domain.Model;
using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;
using SpotTrack.Platform.Alerts.Domain.Model.Commands;
using SpotTrack.Platform.Alerts.Domain.Repositories;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Alerts.Application.Internal.CommandServices;

public class AlertCommandService(
    IAlertRepository alertRepository,
    IUnitOfWork unitOfWork)
    : IAlertCommandService
{
    public async Task<Result<Alert>> Handle(CreateAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = new Alert(command);

        try
        {
            await alertRepository.AddAsync(alert, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return Result<Alert>.Failure(AlertError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Alert>.Failure(AlertError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<Alert>.Failure(AlertError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
        if (alert is null)
            return Result<Alert>.Failure(AlertError.AlertNotFound, "Alert not found.");

        if (alert.AdminId != command.AdminId)
            return Result<Alert>.Failure(AlertError.AccessDenied, "Access denied.");

        alert.Resolve();

        try
        {
            alertRepository.Update(alert);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Alert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return Result<Alert>.Failure(AlertError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Alert>.Failure(AlertError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<Alert>.Failure(AlertError.InternalServerError, "An unexpected error occurred.");
        }
    }
}
