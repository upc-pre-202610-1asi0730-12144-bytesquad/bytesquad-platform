using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Model.Events;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Monitoring.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.CommandServices;

public class AnomalyCommandService(
    IAnomalyRepository anomalyRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<MonitoringMessages> localizer)
    : IAnomalyCommandService
{
    public async Task<Result<Anomaly>> Handle(ReportAnomalyCommand command, CancellationToken cancellationToken)
    {
        Anomaly anomaly;
        try
        {
            anomaly = new Anomaly(command);
        }
        catch (ArgumentException)
        {
            return Result<Anomaly>.Failure(
                MonitoringError.InvalidAnomalyData,
                localizer[nameof(MonitoringError.InvalidAnomalyData)]);
        }

        try
        {
            await anomalyRepository.AddAsync(anomaly, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await mediator.PublishAsync(
                EquipmentAnomalyReportSubmittedEvent.FromAnomaly(anomaly),
                cancellationToken);
            return Result<Anomaly>.Success(anomaly);
        }
        catch (OperationCanceledException)
        {
            return Result<Anomaly>.Failure(
                MonitoringError.OperationCancelled,
                localizer[nameof(MonitoringError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Anomaly>.Failure(
                MonitoringError.DatabaseError,
                localizer[nameof(MonitoringError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Anomaly>.Failure(
                MonitoringError.InternalServerError,
                localizer[nameof(MonitoringError.InternalServerError)]);
        }
    }
}
