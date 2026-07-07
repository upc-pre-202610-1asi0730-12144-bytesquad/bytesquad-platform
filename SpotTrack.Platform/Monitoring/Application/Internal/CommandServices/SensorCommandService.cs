using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.CommandServices;

public class SensorCommandService(
    ISensorRepository sensorRepository,
    IUnitOfWork unitOfWork)
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
            return Result<Sensor>.Failure(MonitoringError.InvalidSensorData, "Invalid sensor data provided.");
        }

        try
        {
            await sensorRepository.AddAsync(sensor, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (OperationCanceledException)
        {
            return Result<Sensor>.Failure(MonitoringError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Sensor>.Failure(MonitoringError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<Sensor>.Failure(MonitoringError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<Sensor>> Handle(CaptureSensorEventCommand command, CancellationToken cancellationToken)
    {
        var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
        if (sensor is null)
            return Result<Sensor>.Failure(MonitoringError.SensorNotFound, "Sensor not found.");

        sensor.CaptureEvent(command.DetectedAt);

        try
        {
            sensorRepository.Update(sensor);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (OperationCanceledException)
        {
            return Result<Sensor>.Failure(MonitoringError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Sensor>.Failure(MonitoringError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<Sensor>.Failure(MonitoringError.InternalServerError, "An unexpected error occurred.");
        }
    }
}
