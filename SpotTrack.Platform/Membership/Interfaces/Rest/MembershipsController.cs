using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Application.QueryServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Domain.Model.Queries;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest;

[ApiController]
[Route("api/v1/memberships")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Membership management endpoints")]
public class MembershipsController(
    IMembershipCommandService membershipCommandService,
    IMembershipQueryService membershipQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("activate")]
    [SwaggerOperation(Summary = "Activate a membership", OperationId = "ActivateMembership")]
    [SwaggerResponse(StatusCodes.Status201Created, "Membership activated successfully", typeof(MembershipResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid membership data provided")]
    public async Task<IActionResult> ActivateMembership(
        [FromBody] ActivateMembershipResource resource,
        CancellationToken cancellationToken)
    {
        var command = ActivateMembershipCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await membershipCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MembershipsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MembershipsActionResultAssembler.ToSuccessActionResult(
            result.Value!, MembershipResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created, this);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get membership by id", OperationId = "GetMembershipById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Membership found", typeof(MembershipResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Membership not found")]
    public async Task<IActionResult> GetMembershipById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var membership = await membershipQueryService.Handle(new GetMembershipByIdQuery(id), cancellationToken);
        if (membership is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, MembershipError.MembershipNotFound, "Membership not found.");
        return MembershipsActionResultAssembler.ToSuccessActionResult(
            membership, MembershipResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK, this);
    }

    [HttpGet("by-client/{clientId:int}")]
    [SwaggerOperation(Summary = "Get all memberships by client id", OperationId = "GetAllMembershipsByClientId")]
    [SwaggerResponse(StatusCodes.Status200OK, "Memberships retrieved successfully", typeof(IEnumerable<MembershipResource>))]
    public async Task<IActionResult> GetAllMembershipsByClientId(
        [FromRoute] int clientId,
        CancellationToken cancellationToken)
    {
        var memberships = await membershipQueryService.Handle(
            new GetAllMembershipsByClientIdQuery(clientId), cancellationToken);
        return Ok(memberships.Select(MembershipResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPut("{id:int}/plan")]
    [SwaggerOperation(Summary = "Upgrade a membership plan", OperationId = "UpgradeMembershipPlan")]
    [SwaggerResponse(StatusCodes.Status200OK, "Membership plan upgraded successfully", typeof(MembershipResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid plan or membership status")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Membership not found")]
    public async Task<IActionResult> UpgradeMembershipPlan(
        [FromRoute] int id,
        [FromBody] UpgradeMembershipPlanResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpgradeMembershipPlanCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await membershipCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MembershipsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MembershipsActionResultAssembler.ToSuccessActionResult(
            result.Value!, MembershipResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK, this);
    }

    [HttpPost("{id:int}/suspend")]
    [SwaggerOperation(Summary = "Suspend a membership", OperationId = "SuspendMembership")]
    [SwaggerResponse(StatusCodes.Status200OK, "Membership suspended successfully", typeof(MembershipResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Membership is not in a suspendable state")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Membership not found")]
    public async Task<IActionResult> SuspendMembership(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new CreateSuspendMembershipCommand(id);
        var result = await membershipCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MembershipsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MembershipsActionResultAssembler.ToSuccessActionResult(
            result.Value!, MembershipResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK, this);
    }

    [HttpPost("{id:int}/renew")]
    [SwaggerOperation(Summary = "Renew a membership", OperationId = "RenewMembership")]
    [SwaggerResponse(StatusCodes.Status200OK, "Membership renewed successfully", typeof(MembershipResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid new end date or membership status")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Membership not found")]
    public async Task<IActionResult> RenewMembership(
        [FromRoute] int id,
        [FromBody] RenewMembershipResource resource,
        CancellationToken cancellationToken)
    {
        var command = RenewMembershipCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await membershipCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MembershipsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MembershipsActionResultAssembler.ToSuccessActionResult(
            result.Value!, MembershipResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK, this);
    }

    [HttpDelete("{id:int}/cancel")]
    [SwaggerOperation(Summary = "Cancel a membership", OperationId = "CancelMembership")]
    [SwaggerResponse(StatusCodes.Status200OK, "Membership cancelled successfully", typeof(MembershipResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Membership is already cancelled or expired")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Membership not found")]
    public async Task<IActionResult> CancelMembership(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new CreateCancelMembershipCommand(id);
        var result = await membershipCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MembershipsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MembershipsActionResultAssembler.ToSuccessActionResult(
            result.Value!, MembershipResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK, this);
    }
}