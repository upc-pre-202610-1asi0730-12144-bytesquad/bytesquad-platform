using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.Queries;

namespace SpotTrack.Platform.Iam.Application.QueryServices;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken);
    Task<User?> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken);
}
