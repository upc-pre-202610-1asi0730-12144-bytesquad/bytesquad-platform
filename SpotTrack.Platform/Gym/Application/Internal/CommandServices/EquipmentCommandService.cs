using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Gyms.Domain.Model;
using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Gyms.Application.Internal.CommandServices;

public class EquipmentCommandService(
    IEquipmentRepository equipmentRepository,
    IGymRepository gymRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<EquipmentMessages> localizer)
    : IEquipmentCommandService
{
    public async Task<Result<Equipment>> Handle(RegisterEquipmentCommand command, CancellationToken cancellationToken)
    {
        if (!await gymRepository.ExistsZoneByIdAsync(command.ZoneId, cancellationToken))
            return Result<Equipment>.Failure(
                EquipmentError.ZoneNotFound,
                localizer[nameof(EquipmentError.ZoneNotFound)]);

        Equipment equipment;
        try
        {
            equipment = new Equipment(command);
            await equipmentRepository.AddAsync(equipment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Equipment>.Success(equipment);
        }
        catch (ArgumentException)
        {
            return Result<Equipment>.Failure(
                EquipmentError.InvalidData,
                localizer[nameof(EquipmentError.InvalidData)]);
        }
        catch (OperationCanceledException)
        {
            return Result<Equipment>.Failure(
                EquipmentError.OperationCancelled,
                localizer[nameof(EquipmentError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Equipment>.Failure(
                EquipmentError.DatabaseError,
                localizer[nameof(EquipmentError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Equipment>.Failure(
                EquipmentError.InternalServerError,
                localizer[nameof(EquipmentError.InternalServerError)]);
        }
    }

    public async Task<Result<Equipment>> Handle(OccupyEquipmentCommand command, CancellationToken cancellationToken)
    {
        var equipment = await equipmentRepository.FindByIdAsync(command.EquipmentId, cancellationToken);
        if (equipment is null)
            return Result<Equipment>.Failure(
                EquipmentError.EquipmentNotFound,
                localizer[nameof(EquipmentError.EquipmentNotFound)]);

        try
        {
            equipment.Occupy();
            equipmentRepository.Update(equipment);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Equipment>.Success(equipment);
        }
        catch (InvalidOperationException)
        {
            return Result<Equipment>.Failure(
                EquipmentError.InvalidEquipmentStatus,
                localizer[nameof(EquipmentError.InvalidEquipmentStatus)]);
        }
        catch (DbUpdateException)
        {
            return Result<Equipment>.Failure(
                EquipmentError.DatabaseError,
                localizer[nameof(EquipmentError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Equipment>.Failure(
                EquipmentError.InternalServerError,
                localizer[nameof(EquipmentError.InternalServerError)]);
        }
    }

    public async Task<Result<Equipment>> Handle(ReleaseEquipmentCommand command, CancellationToken cancellationToken)
    {
        var equipment = await equipmentRepository.FindByIdAsync(command.EquipmentId, cancellationToken);
        if (equipment is null)
            return Result<Equipment>.Failure(
                EquipmentError.EquipmentNotFound,
                localizer[nameof(EquipmentError.EquipmentNotFound)]);

        try
        {
            equipment.Release();
            equipmentRepository.Update(equipment);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Equipment>.Success(equipment);
        }
        catch (InvalidOperationException)
        {
            return Result<Equipment>.Failure(
                EquipmentError.InvalidEquipmentStatus,
                localizer[nameof(EquipmentError.InvalidEquipmentStatus)]);
        }
        catch (DbUpdateException)
        {
            return Result<Equipment>.Failure(
                EquipmentError.DatabaseError,
                localizer[nameof(EquipmentError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Equipment>.Failure(
                EquipmentError.InternalServerError,
                localizer[nameof(EquipmentError.InternalServerError)]);
        }
    }
}
