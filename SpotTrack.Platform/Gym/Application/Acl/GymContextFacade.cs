using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Interfaces.Acl;

namespace SpotTrack.Platform.Gyms.Application.Acl;

public class GymContextFacade(IEquipmentCommandService equipmentCommandService) : IGymContextFacade
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
}
