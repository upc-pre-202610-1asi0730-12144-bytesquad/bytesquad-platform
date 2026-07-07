using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Application.QueryServices;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.Queries;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Iam.Interfaces.Rest.Resources;
using SpotTrack.Platform.Iam.Interfaces.Rest.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/users")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
[SwaggerTag("User management endpoints")]
public class UsersController(IUserQueryService userQueryService) : ControllerBase
{
    [HttpGet]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Returns all registered users. Requires Admin role.",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of users", typeof(IEnumerable<UserResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await userQueryService.Handle(new GetAllUsersQuery(), cancellationToken);
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("me")]
    [SwaggerOperation(
        Summary = "Get the authenticated user's IAM record",
        Description = "Returns the raw IAM record (Id, Username, Role) of the currently authenticated user.",
        OperationId = "GetCurrentUser")]
    [SwaggerResponse(StatusCodes.Status200OK, "Current user", typeof(UserResource))]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = ((User)HttpContext.Items["User"]!).Id;
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), cancellationToken);
        if (user is null) return NotFound();
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    [HttpGet("{userId:int}")]
    [SwaggerOperation(
        Summary = "Get a user by id",
        Description = "Returns the IAM record for the given user. Admins can query any user; non-admins can only query themselves.",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "User found", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> GetUserById(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var authenticated = (User)HttpContext.Items["User"]!;
        if (authenticated.Role != UserRole.Admin && authenticated.Id != userId)
            return Forbid();

        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), cancellationToken);
        if (user is null) return NotFound();
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }
}
