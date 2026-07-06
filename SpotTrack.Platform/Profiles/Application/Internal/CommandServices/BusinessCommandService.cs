using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Profiles.Application.CommandServices;
using SpotTrack.Platform.Profiles.Domain.Model;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Profiles.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Application.Internal.CommandServices;

public class BusinessCommandService(
    IBusinessRepository businessRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ProfilesMessages> localizer)
    : IBusinessCommandService
{
    public async Task<Result<Business>> Handle(ProvisionBusinessCommand command, CancellationToken cancellationToken)
    {
        Business business;
        try
        {
            business = new Business(command.AdminId, command.CompanyName, command.Ruc,
                command.LegalStructure, command.CompanyPhone, command.CompanyEmail);
        }
        catch (ArgumentException)
        {
            return Result<Business>.Failure(
                ProfilesError.InvalidProfileData,
                localizer[nameof(ProfilesError.InvalidProfileData)]);
        }

        try
        {
            await businessRepository.AddAsync(business, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Business>.Success(business);
        }
        catch (OperationCanceledException)
        {
            return Result<Business>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Business>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Business>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }
}
