using SpotTrack.Platform.Iam.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
    Task<int?> ValidateToken(string token);
}
