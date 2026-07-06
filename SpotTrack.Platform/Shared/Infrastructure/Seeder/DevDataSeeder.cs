using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Iam.Application.CommandServices;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Domain.Repositories;
using SpotTrack.Platform.Iam.Interfaces.Acl;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Application.CommandServices;
using SpotTrack.Platform.Profiles.Application.QueryServices;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Domain.Model.Queries;
using SpotTrack.Platform.Profiles.Domain.Repositories;

namespace SpotTrack.Platform.Shared.Infrastructure.Seeder;

/// <summary>
///     Development-only convenience data: one Admin (with a gym, a branch, a zone,
///     two equipment items and an active platform membership) and one Client
///     (profile-complete and associated to that gym as their active gym).
///     Runs once at startup in Development; skipped if the admin account already exists.
/// </summary>
public static class DevDataSeeder
{
    public const string AdminUsername = "admin@spottrack.test";
    public const string AdminPassword = "Admin123!";
    public const string ClientUsername = "client@spottrack.test";
    public const string ClientPassword = "Client123!";

    public static async Task SeedAsync(IServiceProvider rootServices)
    {
        using var scope = rootServices.CreateScope();
        var sp = scope.ServiceProvider;
        var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("DevDataSeeder");

        var userRepository = sp.GetRequiredService<IUserRepository>();
        if (await userRepository.ExistsByUsernameAsync(AdminUsername))
        {
            logger.LogInformation("DevDataSeeder: demo data already present, skipping.");
            return;
        }

        var userCommandService = sp.GetRequiredService<IUserCommandService>();
        var iamContextFacade = sp.GetRequiredService<IIamContextFacade>();
        var adminRepository = sp.GetRequiredService<IAdminRepository>();
        var adminCommandService = sp.GetRequiredService<IAdminCommandService>();
        var clientQueryService = sp.GetRequiredService<IClientQueryService>();
        var clientCommandService = sp.GetRequiredService<IClientCommandService>();
        var gymCommandService = sp.GetRequiredService<IGymCommandService>();
        var equipmentCommandService = sp.GetRequiredService<IEquipmentCommandService>();
        var membershipCommandService = sp.GetRequiredService<IMembershipCommandService>();

        await userCommandService.Handle(new SignUpCommand(AdminUsername, AdminPassword, "Admin"), default);
        await userCommandService.Handle(new SignUpCommand(ClientUsername, ClientPassword, "Client"), default);

        var adminUserId = await iamContextFacade.FetchUserIdByUsernameAsync(AdminUsername);
        var clientUserId = await iamContextFacade.FetchUserIdByUsernameAsync(ClientUsername);

        var admins = await adminRepository.ListAsync();
        var adminProfileId = admins.First(a => a.UserId == adminUserId).Id;
        await adminCommandService.Handle(
            new UpdateAdminProfileCommand(adminProfileId, "Demo", "Admin", "999999999"), default);

        var client = await clientQueryService.Handle(new GetClientByUserIdQuery(clientUserId), default);
        await clientCommandService.Handle(
            new UpdateClientProfileCommand(client!.Id, "Demo", "Client", "988888888"), default);

        var gym = (await gymCommandService.Handle(
            new CreateGymCommand(adminUserId, "SpotTrack Demo Gym", "Av. Demo 123", "Miraflores", "Lima"),
            default)).Value!;

        var branch = (await gymCommandService.Handle(
            new CreateBranchCommand(gym.Id, adminUserId, "Sede Principal", "Av. Demo 123", "Miraflores", "Lima"),
            default)).Value!;

        var zone = (await gymCommandService.Handle(
            new CreateZoneCommand(gym.Id, branch.Id, "Zona de Pesas", 20),
            default)).Value!;

        await equipmentCommandService.Handle(
            new RegisterEquipmentCommand("Caminadora", "Technogym Run 900", zone.Id), default);
        await equipmentCommandService.Handle(
            new RegisterEquipmentCommand("Bicicleta estática", "Technogym Bike 700", zone.Id), default);

        await membershipCommandService.Handle(
            new CreateActivateMembershipCommand(
                adminUserId, EMembershipPlan.Premium, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1)),
            default);

        await clientCommandService.Handle(
            new AssociateClientWithGymCommand(client.Id, gym.Id), default);

        logger.LogInformation(
            "DevDataSeeder: demo data created. Admin='{AdminUsername}'/'{AdminPassword}', Client='{ClientUsername}'/'{ClientPassword}', GymId={GymId}.",
            AdminUsername, AdminPassword, ClientUsername, ClientPassword, gym.Id);
    }
}
