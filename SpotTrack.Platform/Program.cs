using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Application.Internal.CommandServices;
using SpotTrack.Platform.Analytics.Application.Internal.QueryServices;
using SpotTrack.Platform.Analytics.Application.QueryServices;
using SpotTrack.Platform.Analytics.Domain.Repositories;
using SpotTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Profiles.Application.Acl;
using SpotTrack.Platform.Profiles.Application.CommandServices;
using SpotTrack.Platform.Profiles.Application.Internal.CommandServices;
using SpotTrack.Platform.Profiles.Application.Internal.QueryServices;
using SpotTrack.Platform.Profiles.Application.QueryServices;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Profiles.Interfaces.Acl;
using SpotTrack.Platform.Resources.Errors;
using SpotTrack.Platform.Resources.Shared;
using SpotTrack.Platform.Shared.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using SpotTrack.Platform.Shared.Infrastructure.Seeder;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
using ProblemDetailsFactory = SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;
using SpotTrack.Platform.Reservations.Application.Acl;
using SpotTrack.Platform.Reservations.Application.CommandServices;
using SpotTrack.Platform.Reservations.Interfaces.Acl;
using SpotTrack.Platform.Reservations.Application.Internal.CommandServices;
using SpotTrack.Platform.Reservations.Application.Internal.QueryServices;
using SpotTrack.Platform.Reservations.Application.QueryServices;
using SpotTrack.Platform.Reservations.Domain.Repositories;
using SpotTrack.Platform.Reservations.Infrastructure.BackgroundServices;
using SpotTrack.Platform.Reservations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Reservations.Resources;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Application.Internal.CommandServices;
using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Application.Internal.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Monitoring.Resources;
using SpotTrack.Platform.Routines.Application.CommandServices;
using SpotTrack.Platform.Routines.Application.Internal.CommandServices;
using SpotTrack.Platform.Routines.Application.Internal.QueryServices;
using SpotTrack.Platform.Routines.Application.QueryServices;
using SpotTrack.Platform.Routines.Domain.Repositories;
using SpotTrack.Platform.Routines.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Routines.Resources;
using SpotTrack.Platform.Iam.Application.Acl;
using SpotTrack.Platform.Iam.Application.CommandServices;
using SpotTrack.Platform.Iam.Application.Internal.CommandServices;
using SpotTrack.Platform.Iam.Application.Internal.QueryServices;
using SpotTrack.Platform.Iam.Application.QueryServices;
using SpotTrack.Platform.Iam.Domain.Repositories;
using SpotTrack.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;
using SpotTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using SpotTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using SpotTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Services;
using SpotTrack.Platform.Iam.Application.Internal.OutboundServices;
using SpotTrack.Platform.Iam.Infrastructure.Email;
using SpotTrack.Platform.Iam.Interfaces.Acl;
using SpotTrack.Platform.Iam.Resources;
using SpotTrack.Platform.Gyms.Application.Acl;
using SpotTrack.Platform.Gyms.Application.Internal.CommandServices;
using SpotTrack.Platform.Gyms.Application.Internal.QueryServices;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Gyms.Interfaces.Acl;
using SpotTrack.Platform.Gyms.Resources;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Application.Internal.CommandServices;
using SpotTrack.Platform.Memberships.Application.Internal.QueryServices;
using SpotTrack.Platform.Memberships.Application.QueryServices;
using SpotTrack.Platform.Memberships.Domain.Repositories;
using SpotTrack.Platform.Memberships.Infrastructure.BackgroundServices;
using SpotTrack.Platform.Memberships.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Memberships.Infrastructure.Stripe;
using SpotTrack.Platform.Memberships.Resources;
using SpotTrack.Platform.Maintenances.Application.CommandServices;
using SpotTrack.Platform.Maintenances.Application.Internal.CommandServices;
using SpotTrack.Platform.Maintenances.Application.Internal.QueryServices;
using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Maintenances.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using SpotTrack.Platform.Maintenances.Resources;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Application.Internal.CommandServices;
using SpotTrack.Platform.Monitoring.Application.Internal.QueryServices;
using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// Add ProblemDetails services
builder.Services.AddProblemDetails();

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Database Context and route EF logs through the app logger pipeline.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();

// Register the custom ProblemDetailsFactory
builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "SpotTrack.Platform",
            Version = "v1",
            Description = "SpotTrack Gym Monitoring Platform API",
            Contact = new OpenApiContact
            {
                Name = "ORION-tech-c",
                Email = "contact@spottrack.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        { [new OpenApiSecuritySchemeReference("bearer", document)] = [] });
    options.EnableAnnotations();
});

// Dependency Injection

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Analytics Bounded Context
builder.Services.AddScoped<IActivityReportRepository, ActivityReportRepository>();
builder.Services.AddScoped<IMaintenanceQuoteRepository, MaintenanceQuoteRepository>();
builder.Services.AddScoped<IROIProjectionRepository, ROIProjectionRepository>();
builder.Services.AddScoped<IActivityReportCommandService, ActivityReportCommandService>();
builder.Services.AddScoped<IMaintenanceQuoteCommandService, MaintenanceQuoteCommandService>();
builder.Services.AddScoped<IROIProjectionCommandService, ROIProjectionCommandService>();
builder.Services.AddScoped<IActivityReportQueryService, ActivityReportQueryService>();
builder.Services.AddScoped<IMaintenanceQuoteQueryService, MaintenanceQuoteQueryService>();
builder.Services.AddScoped<IROIProjectionQueryService, ROIProjectionQueryService>();

// Profiles Bounded Context
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientGymAssociationRepository, ClientGymAssociationRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IClientCommandService, ClientCommandService>();
builder.Services.AddScoped<IAdminCommandService, AdminCommandService>();
builder.Services.AddScoped<IBusinessCommandService, BusinessCommandService>();
builder.Services.AddScoped<IClientQueryService, ClientQueryService>();
builder.Services.AddScoped<IAdminQueryService, AdminQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationCommandService, ReservationCommandService>();
builder.Services.AddScoped<IReservationQueryService, ReservationQueryService>();
builder.Services.AddScoped<IEquipmentUsageStatsQueryService, EquipmentUsageStatsQueryService>();
builder.Services.AddScoped<IReservationsMembershipContextFacade, ReservationsMembershipContextFacade>();
builder.Services.AddSingleton<IStringLocalizer<ReservationMessages>, StringLocalizer<ReservationMessages>>();

// Routines Bounded Context
builder.Services.AddScoped<IRoutineRepository, RoutineRepository>();
builder.Services.AddScoped<IRoutineCommandService, RoutineCommandService>();
builder.Services.AddScoped<IRoutineQueryService, RoutineQueryService>();
builder.Services.AddScoped<IRoutineSessionRepository, RoutineSessionRepository>();
builder.Services.AddScoped<IRoutineSessionCommandService, RoutineSessionCommandService>();
builder.Services.AddScoped<IRoutineSessionQueryService, RoutineSessionQueryService>();
builder.Services.AddSingleton<IStringLocalizer<RoutinesMessages>, StringLocalizer<RoutinesMessages>>();

// Monitoring Bounded Context
builder.Services.AddScoped<IAnomalyRepository, AnomalyRepository>();
builder.Services.AddScoped<IAnomalyCommandService, AnomalyCommandService>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorCommandService, SensorCommandService>();
builder.Services.AddScoped<ISensorQueryService, SensorQueryService>();
builder.Services.AddSingleton<IStringLocalizer<MonitoringMessages>, StringLocalizer<MonitoringMessages>>();

// Gym Bounded Context
builder.Services.AddScoped<IGymRepository, GymRepository>();
builder.Services.AddScoped<IAuthorizedDniRepository, AuthorizedDniRepository>();
builder.Services.AddScoped<IGymCommandService, GymCommandService>();
builder.Services.AddScoped<IGymQueryService, GymQueryService>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IEquipmentCommandService, EquipmentCommandService>();
builder.Services.AddScoped<IEquipmentQueryService, EquipmentQueryService>();
builder.Services.AddScoped<IGymContextFacade, GymContextFacade>();
builder.Services.AddScoped<IMembershipContextFacade, MembershipContextFacade>();
builder.Services.AddSingleton<IStringLocalizer<GymMessages>, StringLocalizer<GymMessages>>();
builder.Services.AddSingleton<IStringLocalizer<EquipmentMessages>, StringLocalizer<EquipmentMessages>>();

// Membership Bounded Context
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
builder.Services.AddScoped<IMembershipCommandService, MembershipCommandService>();
builder.Services.AddScoped<IMembershipQueryService, MembershipQueryService>();
builder.Services.AddScoped<IBranchAccessRepository, BranchAccessRepository>();
builder.Services.AddScoped<IBranchAccessCommandService, BranchAccessCommandService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentCommandService, PaymentCommandService>();
builder.Services.AddSingleton<IStringLocalizer<MembershipMessages>, StringLocalizer<MembershipMessages>>();
builder.Services.AddHostedService<MembershipExpirationBackgroundService>();
builder.Services.AddHostedService<ReservationTimerExpiryBackgroundService>();

// Maintenance Bounded Context
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddScoped<IMaintenanceCommandService, MaintenanceCommandService>();
builder.Services.AddScoped<IMaintenanceQueryService, MaintenanceQueryService>();
builder.Services.AddSingleton<IStringLocalizer<MaintenanceMessages>, StringLocalizer<MaintenanceMessages>>();
builder.Services.AddScoped<ITechnicalTicketRepository, TechnicalTicketRepository>();
builder.Services.AddScoped<ITechnicalTicketCommandService, TechnicalTicketCommandService>();
builder.Services.AddScoped<ITechnicalTicketQueryService, TechnicalTicketQueryService>();
builder.Services.AddScoped<IMaintenanceJobRepository, MaintenanceJobRepository>();
builder.Services.AddScoped<IMaintenanceJobCommandService, MaintenanceJobCommandService>();
builder.Services.AddScoped<IMaintenanceLogRepository, MaintenanceLogRepository>();
builder.Services.AddScoped<IMaintenanceLogCommandService, MaintenanceLogCommandService>();
builder.Services.AddScoped<IMaintenanceLogQueryService, MaintenanceLogQueryService>();
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<ITechnicianCommandService, TechnicianCommandService>();
builder.Services.AddScoped<ITechnicianQueryService, TechnicianQueryService>();

// Monitoring Bounded Context
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISessionTrackerRepository, SessionTrackerRepository>();
builder.Services.AddScoped<IAnomalyRepository, AnomalyRepository>();
builder.Services.AddScoped<ISensorCommandService, SensorCommandService>();
builder.Services.AddScoped<ISessionTrackerCommandService, SessionTrackerCommandService>();
builder.Services.AddScoped<IAnomalyCommandService, AnomalyCommandService>();
builder.Services.AddScoped<ISensorQueryService, SensorQueryService>();
builder.Services.AddScoped<ISessionTrackerQueryService, SessionTrackerQueryService>();
builder.Services.AddScoped<IAnomalyQueryService, AnomalyQueryService>();

// IAM Bounded Context
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPendingRegistrationRepository, PendingRegistrationRepository>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IPendingRegistrationCommandService, PendingRegistrationCommandService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddSingleton<IStringLocalizer<IamMessages>, StringLocalizer<IamMessages>>();

// Mediator Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

// Apply pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// Development-only demo data (one Admin with a gym + one Client ready to test)
if (app.Environment.IsDevelopment())
    await DevDataSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRequestAuthorization();

if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();