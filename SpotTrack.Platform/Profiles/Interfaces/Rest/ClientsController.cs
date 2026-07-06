using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using SpotTrack.Platform.Profiles.Application.CommandServices;
using SpotTrack.Platform.Profiles.Application.QueryServices;
using SpotTrack.Platform.Profiles.Domain.Model;
using SpotTrack.Platform.Profiles.Domain.Model.Queries;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest;

[ApiController]
[Route("api/v1/profiles/clients")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
[SwaggerTag("Client profile management endpoints")]
public class ClientsController(
    IClientCommandService clientCommandService,
    IClientQueryService clientQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new client profile",
        Description = "Creates a new client profile for the given user. Returns 409 if the email is already registered.",
        OperationId = "CreateClient")]
    [SwaggerResponse(StatusCodes.Status201Created, "Client profile created successfully", typeof(ClientResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid client data provided")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Email address is already registered")]
    public async Task<IActionResult> CreateClient(
        [FromBody] CreateClientResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateClientCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await clientCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return ProfilesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return ProfilesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ClientResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpGet("me")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Get my client profile",
        Description = "Returns the client profile of the currently authenticated user.",
        OperationId = "GetMyProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "Profile found", typeof(ClientResource))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAuthenticatedUserId()!.Value;
        var client = await clientQueryService.Handle(new GetClientByUserIdQuery(userId), cancellationToken);
        if (client is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ProfilesError.ClientNotFound, "Client not found.");
        return Ok(ClientResourceFromEntityAssembler.ToResourceFromEntity(client));
    }

    [HttpPut("me")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Update my client profile",
        Description = "Updates personal data (first name, last name, phone number) for the currently authenticated client.",
        OperationId = "UpdateMyProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "Profile updated successfully", typeof(ClientResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid profile data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> UpdateMyProfile(
        [FromBody] UpdateClientProfileResource resource,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAuthenticatedUserId()!.Value;
        var client = await clientQueryService.Handle(new GetClientByUserIdQuery(userId), cancellationToken);
        if (client is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ProfilesError.ClientNotFound, "Client not found.");

        var command = UpdateClientProfileCommandFromResourceAssembler.ToCommandFromResource(client.Id, resource);
        var result = await clientCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return ProfilesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return ProfilesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ClientResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpPost("me/gym-associations")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Associate myself with a gym",
        Description = "Associates the currently authenticated client with a gym. The first association becomes the active gym automatically.",
        OperationId = "AssociateGym")]
    [SwaggerResponse(StatusCodes.Status201Created, "Association created successfully", typeof(ClientGymAssociationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Client profile is incomplete")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Already associated with this gym")]
    public async Task<IActionResult> AssociateGym(
        [FromBody] AssociateGymResource resource,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAuthenticatedUserId()!.Value;
        var client = await clientQueryService.Handle(new GetClientByUserIdQuery(userId), cancellationToken);
        if (client is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ProfilesError.ClientNotFound, "Client not found.");

        var command = AssociateClientWithGymCommandFromResourceAssembler.ToCommandFromResource(client.Id, resource);
        var result = await clientCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return ProfilesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return ProfilesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ClientGymAssociationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpGet("me/gym-associations")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Get my gym associations",
        Description = "Returns the gyms the currently authenticated client is associated with.",
        OperationId = "GetMyGymAssociations")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of gym associations", typeof(IEnumerable<ClientGymAssociationResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> GetMyGymAssociations(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAuthenticatedUserId()!.Value;
        var client = await clientQueryService.Handle(new GetClientByUserIdQuery(userId), cancellationToken);
        if (client is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ProfilesError.ClientNotFound, "Client not found.");

        var associations = await clientQueryService.Handle(
            new GetClientGymAssociationsQuery(client.Id), cancellationToken);
        var resources = associations.Select(ClientGymAssociationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPatch("me/active-gym")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Change my active gym",
        Description = "Switches the currently authenticated client's active gym to one they are already associated with.",
        OperationId = "ChangeActiveGym")]
    [SwaggerResponse(StatusCodes.Status200OK, "Active gym changed successfully", typeof(ClientGymAssociationResource))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found, or client is not associated with that gym")]
    public async Task<IActionResult> ChangeActiveGym(
        [FromBody] ChangeActiveGymResource resource,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetAuthenticatedUserId()!.Value;
        var client = await clientQueryService.Handle(new GetClientByUserIdQuery(userId), cancellationToken);
        if (client is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ProfilesError.ClientNotFound, "Client not found.");

        var command = ChangeActiveGymCommandFromResourceAssembler.ToCommandFromResource(client.Id, resource);
        var result = await clientCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return ProfilesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return ProfilesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ClientGymAssociationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpGet("{clientId:int}")]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Get a client profile by ID",
        Description = "Returns the client profile matching the given ID, or 404 if not found.",
        OperationId = "GetClientById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Client profile found", typeof(ClientResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> GetClientById(
        [FromRoute] int clientId,
        CancellationToken cancellationToken)
    {
        var client = await clientQueryService.Handle(new GetClientByIdQuery(clientId), cancellationToken);
        if (client is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ProfilesError.ClientNotFound, "Client not found.");
        return Ok(ClientResourceFromEntityAssembler.ToResourceFromEntity(client));
    }

    [HttpGet]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Get all client profiles",
        Description = "Returns the list of all registered client profiles.",
        OperationId = "GetAllClients")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of client profiles", typeof(IEnumerable<ClientResource>))]
    public async Task<IActionResult> GetAllClients(CancellationToken cancellationToken)
    {
        var clients = await clientQueryService.Handle(new GetAllClientsQuery(), cancellationToken);
        var resources = clients.Select(ClientResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPut("{clientId:int}")]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Update a client profile",
        Description = "Updates the first name, last name and phone number of the given client.",
        OperationId = "UpdateClientProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "Client profile updated successfully", typeof(ClientResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid profile data provided")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> UpdateClientProfile(
        [FromRoute] int clientId,
        [FromBody] UpdateClientProfileResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateClientProfileCommandFromResourceAssembler.ToCommandFromResource(clientId, resource);
        var result = await clientCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return ProfilesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return ProfilesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ClientResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }
}
