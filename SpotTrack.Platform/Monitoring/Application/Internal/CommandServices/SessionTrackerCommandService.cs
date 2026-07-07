using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.CommandServices;

public class SessionTrackerCommandService(
    ISessionTrackerRepository sessionTrackerRepository,
    IUnitOfWork unitOfWork)
    : ISessionTrackerCommandService
{
    public async Task<Result<SessionTracker>> Handle(CreateSessionTrackerCommand command, CancellationToken cancellationToken)
    {
        SessionTracker tracker;
        try
        {
            tracker = new SessionTracker(command);
        }
        catch (ArgumentException)
        {
            return Result<SessionTracker>.Failure(MonitoringError.InvalidTrackerData, "Invalid session tracker data provided.");
        }

        try
        {
            await sessionTrackerRepository.AddAsync(tracker, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SessionTracker>.Success(tracker);
        }
        catch (OperationCanceledException)
        {
            return Result<SessionTracker>.Failure(MonitoringError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<SessionTracker>.Failure(MonitoringError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<SessionTracker>.Failure(MonitoringError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<SessionTracker>> Handle(EndSessionTrackerCommand command, CancellationToken cancellationToken)
    {
        var tracker = await sessionTrackerRepository.FindByIdAsync(command.SessionTrackerId, cancellationToken);
        if (tracker is null)
            return Result<SessionTracker>.Failure(MonitoringError.SessionTrackerNotFound, "Session tracker not found.");

        try
        {
            tracker.End();
        }
        catch (InvalidOperationException ex)
        {
            return Result<SessionTracker>.Failure(MonitoringError.InvalidTrackerData, ex.Message);
        }

        try
        {
            sessionTrackerRepository.Update(tracker);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SessionTracker>.Success(tracker);
        }
        catch (OperationCanceledException)
        {
            return Result<SessionTracker>.Failure(MonitoringError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<SessionTracker>.Failure(MonitoringError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<SessionTracker>.Failure(MonitoringError.InternalServerError, "An unexpected error occurred.");
        }
    }
}
