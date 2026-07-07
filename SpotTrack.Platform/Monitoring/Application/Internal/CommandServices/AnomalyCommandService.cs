using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Model.Events;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.CommandServices;

public class AnomalyCommandService(
    IAnomalyRepository anomalyRepository,
    ISensorRepository sensorRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IAnomalyCommandService
{
    public async Task<Result<Anomaly>> Handle(ReportAnomalyCommand command, CancellationToken cancellationToken)
    {
        var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
        if (sensor is null)
            return Result<Anomaly>.Failure(MonitoringError.SensorNotFound, "Sensor not found.");

        var anomaly = new Anomaly(command, sensor.AdminId);

        try
        {
            await anomalyRepository.AddAsync(anomaly, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await mediator.PublishAsync(AnomalyReportedEvent.FromAnomaly(anomaly), cancellationToken);
            return Result<Anomaly>.Success(anomaly);
        }
        catch (OperationCanceledException)
        {
            return Result<Anomaly>.Failure(MonitoringError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Anomaly>.Failure(MonitoringError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<Anomaly>.Failure(MonitoringError.InternalServerError, "An unexpected error occurred.");
        }
    }
}
