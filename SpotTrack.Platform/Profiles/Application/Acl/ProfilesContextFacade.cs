using SpotTrack.Platform.Profiles.Application.CommandServices;
using SpotTrack.Platform.Profiles.Application.QueryServices;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Domain.Model.Queries;
using SpotTrack.Platform.Profiles.Interfaces.Acl;

namespace SpotTrack.Platform.Profiles.Application.Acl;

public class ProfilesContextFacade(
    IClientCommandService clientCommandService,
    IAdminCommandService adminCommandService,
    IClientQueryService clientQueryService,
    IAdminQueryService adminQueryService)
    : IProfilesContextFacade
{
    public async Task<int> CreateClientAsync(int userId, string email, string firstName,
        string lastName, string phoneNumber, string dni)
    {
        var command = new CreateClientCommand(userId, email, firstName, lastName, phoneNumber, dni);
        var result = await clientCommandService.Handle(command, CancellationToken.None);
        return result.IsFailure ? 0 : result.Value!.Id;
    }

    public async Task<int> CreateAdminAsync(int userId, string email, string firstName,
        string lastName, string phoneNumber, string dni)
    {
        var command = new CreateAdminCommand(userId, email, firstName, lastName, phoneNumber, dni);
        var result = await adminCommandService.Handle(command, CancellationToken.None);
        return result.IsFailure ? 0 : result.Value!.Id;
    }

    public async Task<int> RegisterClientAsync(int userId)
    {
        var command = new RegisterClientCommand(userId);
        var result = await clientCommandService.Handle(command, CancellationToken.None);
        return result.IsFailure ? 0 : result.Value!.Id;
    }

    public async Task<int> RegisterAdminAsync(int userId)
    {
        var command = new RegisterAdminCommand(userId);
        var result = await adminCommandService.Handle(command, CancellationToken.None);
        return result.IsFailure ? 0 : result.Value!.Id;
    }

    public async Task<int> FetchClientIdByEmailAsync(string email)
    {
        var client = await clientQueryService.Handle(
            new GetClientByEmailQuery(email), CancellationToken.None);
        return client?.Id ?? 0;
    }

    public async Task<int> FetchAdminIdByEmailAsync(string email)
    {
        var admin = await adminQueryService.Handle(
            new GetAdminByEmailQuery(email), CancellationToken.None);
        return admin?.Id ?? 0;
    }
}