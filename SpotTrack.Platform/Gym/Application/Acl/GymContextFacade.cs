using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Interfaces.Acl;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Gyms.Application.Acl;

public class GymContextFacade(
    IEquipmentCommandService equipmentCommandService,
    IGymRepository gymRepository,
    IUnitOfWork unitOfWork) : IGymContextFacade
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

    public async Task<IEnumerable<int>> GetEquipmentIdsByAdminIdAsync(
        int adminId,
        CancellationToken cancellationToken = default)
        => await gymRepository.FindAllEquipmentIdsByAdminIdAsync(adminId, cancellationToken);

    public async Task<int> CreateGymWithBranchAsync(
        int adminId, string gymName, string branchName,
        string street, string district, string city,
        CancellationToken cancellationToken = default)
    {
        if (await gymRepository.ExistsByAdminIdAsync(adminId, cancellationToken))
            return 0;

        var gym = new Gym(new CreateGymCommand(adminId, gymName));
        await gymRepository.AddAsync(gym, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        // Branch seeding bypasses GymCommandService to avoid the branch-limit check,
        // which would fail here because the membership is not yet activated at this
        // point in the PaymentConfirmedEventHandler cascade.
        gym.AddBranch(branchName, street, district, city);
        gymRepository.Update(gym);
        await unitOfWork.CompleteAsync(cancellationToken);

        return gym.Id;
    }
}
