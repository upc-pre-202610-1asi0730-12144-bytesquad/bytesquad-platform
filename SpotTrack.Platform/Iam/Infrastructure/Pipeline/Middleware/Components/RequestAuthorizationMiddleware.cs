using SpotTrack.Platform.Iam.Application.Internal.OutboundServices;
using SpotTrack.Platform.Iam.Application.QueryServices;
using SpotTrack.Platform.Iam.Domain.Model.Queries;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;

namespace SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ITokenService tokenService,
        IUserQueryService userQueryService)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await next(context);
            return;
        }

        if (endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() == null)
        {
            await next(context);
            return;
        }

        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
        var token = authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
            ? authHeader["Bearer ".Length..]
            : null;

        if (token is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var userId = await tokenService.ValidateToken(token);
        if (userId is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var user = await userQueryService.Handle(new GetUserByIdQuery(userId.Value), CancellationToken.None);
        if (user is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.Items["User"] = user;
        await next(context);
    }
}
