using SpotTrack.Platform.Iam.Application.CommandServices;
using SpotTrack.Platform.Iam.Application.QueryServices;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Domain.Model.Queries;
using SpotTrack.Platform.Iam.Interfaces.Acl;

namespace SpotTrack.Platform.Iam.Application.Acl;

public class IamContextFacade(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService)
    : IIamContextFacade
{
    public async Task<int> CreateUserAsync(string username, string password, string role)
    {
        var command = new SignUpCommand(username, password, role);
        var result = await userCommandService.Handle(command, CancellationToken.None);
        if (result.IsFailure) return 0;
        var user = await userQueryService.Handle(new GetUserByUsernameQuery(username), CancellationToken.None);
        return user?.Id ?? 0;
    }

    public async Task<int> FetchUserIdByUsernameAsync(string username)
    {
        var user = await userQueryService.Handle(new GetUserByUsernameQuery(username), CancellationToken.None);
        return user?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserIdAsync(int userId)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), CancellationToken.None);
        return user?.Username ?? string.Empty;
    }
}
