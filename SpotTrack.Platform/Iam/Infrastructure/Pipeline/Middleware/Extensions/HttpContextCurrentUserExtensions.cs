using SpotTrack.Platform.Iam.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
///     Reads the authenticated User stored by RequestAuthorizationMiddleware in HttpContext.Items["User"].
/// </summary>
public static class HttpContextCurrentUserExtensions
{
    public static User? GetAuthenticatedUser(this HttpContext context) =>
        context.Items["User"] as User;

    public static int? GetAuthenticatedUserId(this HttpContext context) =>
        context.GetAuthenticatedUser()?.Id;
}
