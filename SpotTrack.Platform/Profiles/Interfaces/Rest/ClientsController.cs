using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{clientId:int}")]
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
