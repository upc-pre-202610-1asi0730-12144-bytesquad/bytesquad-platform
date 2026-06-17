using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Application.QueryServices;
using SpotTrack.Platform.Iam.Domain.Model.Queries;
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
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Returns all registered users. Requires authentication.",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of users", typeof(IEnumerable<UserResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await userQueryService.Handle(new GetAllUsersQuery(), cancellationToken);
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
