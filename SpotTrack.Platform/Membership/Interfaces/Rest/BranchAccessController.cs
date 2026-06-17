using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest;

[ApiController]
[Route("api/v1/branch-accesses")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Branch access management endpoints")]
public class BranchAccessController(
    IBranchAccessCommandService branchAccessCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("grant")]
    [SwaggerOperation(
        Summary = "Grant branch access",
        Description = "Evaluates the referenced membership and records a Granted or Denied branch access decision.",
        OperationId = "GrantBranchAccess")]
    [SwaggerResponse(StatusCodes.Status201Created, "Branch access decision recorded successfully", typeof(BranchAccessResource))]
    public async Task<IActionResult> GrantBranchAccess(
        [FromBody] GrantBranchAccessResource resource,
        CancellationToken cancellationToken)
    {
        var command = GrantBranchAccessCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await branchAccessCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return BranchAccessActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return BranchAccessActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            BranchAccessResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
