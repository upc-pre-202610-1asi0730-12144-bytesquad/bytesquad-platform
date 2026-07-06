using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Interfaces.Acl;

namespace SpotTrack.Platform.Gyms.Application.Acl;

public class GymContextFacade(
    IEquipmentCommandService equipmentCommandService,
    IGymRepository gymRepository,
    IGymCommandService gymCommandService) : IGymContextFacade
{
    public async Task<bool> OccupyEquipmentAsync(int equipmentId)
    {
        var command = new OccupyEquipmentCommand(equipmentId);
        var result = await equipmentCommandService.Handle(command, CancellationToken.None);
        return !result.IsFailure;
    }

    public async Task<bool> ReleaseEquipmentAsync(int equipmentId)
    {
        var command = new ReleaseEquipmentCommand(equipmentId);
        var result = await equipmentCommandService.Handle(command, CancellationToken.None);
        return !result.IsFailure;
    }

    public async Task<bool> MarkEquipmentOutOfServiceAsync(int equipmentId)
    {
        var command = new MarkEquipmentOutOfServiceCommand(equipmentId);
        var result = await equipmentCommandService.Handle(command, CancellationToken.None);
        return !result.IsFailure;
    }

    public async Task<bool> MarkEquipmentAvailableAsync(int equipmentId)
    {
        var command = new MarkEquipmentAvailableCommand(equipmentId);
        var result = await equipmentCommandService.Handle(command, CancellationToken.None);
        return !result.IsFailure;
    }

    public async Task<int?> GetAdminIdByEquipmentIdAsync(
        int equipmentId,
        CancellationToken cancellationToken)
        => await gymRepository.FindAdminIdByEquipmentIdAsync(equipmentId, cancellationToken);

    public async Task<int> CreateGymAsync(int adminId, string name, string street, string district, string city,
        CancellationToken cancellationToken = default)
    {
        if (await gymRepository.ExistsByAdminIdAsync(adminId, cancellationToken))
            return 0;

        var command = new CreateGymCommand(adminId, name, street, district, city);
        var result = await gymCommandService.Handle(command, cancellationToken);
        return result.IsFailure ? 0 : result.Value!.Id;
    }
}
