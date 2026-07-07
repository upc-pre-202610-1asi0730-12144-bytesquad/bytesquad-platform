using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Monitoring.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.CommandServices;

public class SensorCommandService(
    ISensorRepository sensorRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<MonitoringMessages> localizer)
    : ISensorCommandService
{
    public async Task<Result<Sensor>> Handle(RegisterSensorCommand command, CancellationToken cancellationToken)
    {
        Sensor sensor;
        try
        {
            sensor = new Sensor(command);
        }
        catch (ArgumentException)
        {
            return Result<Sensor>.Failure(
                MonitoringError.InvalidSensorData,
                localizer[nameof(MonitoringError.InvalidSensorData)]);
        }

        return await PersistAsync(sensor, cancellationToken);
    }

    public async Task<Result<Sensor>> Handle(MarkSensorDisconnectedCommand command, CancellationToken cancellationToken)
    {
        var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
        if (sensor is null)
            return Result<Sensor>.Failure(
                MonitoringError.SensorNotFound,
                localizer[nameof(MonitoringError.SensorNotFound)]);

        try
        {
            sensor.MarkDisconnected();
        }
        catch (InvalidOperationException ex)
        {
            return Result<Sensor>.Failure(MonitoringError.InvalidSensorStatus, ex.Message);
        }

        return await PersistAsync(sensor, cancellationToken, isNew: false);
    }

    public async Task<Result<Sensor>> Handle(MarkSensorReconnectedCommand command, CancellationToken cancellationToken)
    {
        var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
        if (sensor is null)
            return Result<Sensor>.Failure(
                MonitoringError.SensorNotFound,
                localizer[nameof(MonitoringError.SensorNotFound)]);

        try
        {
            sensor.MarkReconnected();
        }
        catch (InvalidOperationException ex)
        {
            return Result<Sensor>.Failure(MonitoringError.InvalidSensorStatus, ex.Message);
        }

        return await PersistAsync(sensor, cancellationToken, isNew: false);
    }

    private async Task<Result<Sensor>> PersistAsync(Sensor sensor, CancellationToken cancellationToken, bool isNew = true)
    {
        try
        {
            if (isNew)
                await sensorRepository.AddAsync(sensor, cancellationToken);
            else
                sensorRepository.Update(sensor);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (OperationCanceledException)
        {
            return Result<Sensor>.Failure(
                MonitoringError.OperationCancelled,
                localizer[nameof(MonitoringError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Sensor>.Failure(
                MonitoringError.DatabaseError,
                localizer[nameof(MonitoringError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Sensor>.Failure(
                MonitoringError.InternalServerError,
                localizer[nameof(MonitoringError.InternalServerError)]);
        }
    }
}
