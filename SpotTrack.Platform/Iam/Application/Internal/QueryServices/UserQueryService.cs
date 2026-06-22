using SpotTrack.Platform.Iam.Application.QueryServices;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.Queries;
using SpotTrack.Platform.Iam.Domain.Repositories;

namespace SpotTrack.Platform.Iam.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken) =>
        await userRepository.FindByIdAsync(query.UserId, cancellationToken);

    public async Task<User?> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken) =>
        await userRepository.FindByUsernameAsync(query.Username, cancellationToken);

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken) =>
        await userRepository.ListAsync(cancellationToken);
}
