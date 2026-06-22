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
[Route("api/v1/profiles/admins")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Admin profile management endpoints")]
public class AdminsController(
    IAdminCommandService adminCommandService,
    IAdminQueryService adminQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new admin profile",
        Description = "Creates a new admin profile for the given user. Returns 409 if the email is already registered.",
        OperationId = "CreateAdmin")]
    [SwaggerResponse(StatusCodes.Status201Created, "Admin profile created successfully", typeof(AdminResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid admin data provided")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Email address is already registered")]
    public async Task<IActionResult> CreateAdmin(
        [FromBody] CreateAdminResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAdminCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await adminCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return ProfilesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return ProfilesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            AdminResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpGet("{adminId:int}")]
    [SwaggerOperation(
        Summary = "Get an admin profile by ID",
        Description = "Returns the admin profile matching the given ID, or 404 if not found.",
        OperationId = "GetAdminById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Admin profile found", typeof(AdminResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Admin profile not found")]
    public async Task<IActionResult> GetAdminById(
        [FromRoute] int adminId,
        CancellationToken cancellationToken)
    {
        var admin = await adminQueryService.Handle(new GetAdminByIdQuery(adminId), cancellationToken);
        if (admin is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ProfilesError.AdminNotFound, "Admin not found.");
        return Ok(AdminResourceFromEntityAssembler.ToResourceFromEntity(admin));
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all admin profiles",
        Description = "Returns the list of all registered admin profiles.",
        OperationId = "GetAllAdmins")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of admin profiles", typeof(IEnumerable<AdminResource>))]
    public async Task<IActionResult> GetAllAdmins(CancellationToken cancellationToken)
    {
        var admins = await adminQueryService.Handle(new GetAllAdminsQuery(), cancellationToken);
        var resources = admins.Select(AdminResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPut("{adminId:int}")]
    [SwaggerOperation(
        Summary = "Update an admin profile",
        Description = "Updates the first name, last name and phone number of the given admin.",
        OperationId = "UpdateAdminProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "Admin profile updated successfully", typeof(AdminResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid profile data provided")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Admin profile not found")]
    public async Task<IActionResult> UpdateAdminProfile(
        [FromRoute] int adminId,
        [FromBody] UpdateAdminProfileResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateAdminProfileCommandFromResourceAssembler.ToCommandFromResource(adminId, resource);
        var result = await adminCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return ProfilesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return ProfilesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            AdminResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }
}
