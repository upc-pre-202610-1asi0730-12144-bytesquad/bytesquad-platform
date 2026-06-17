using SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using SpotTrack.Platform.Routines.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using SpotTrack.Platform.Reservations.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context for the Learning Center Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <summary>
    ///     On creating the database model
    /// </summary>
    /// <remarks>
    ///     This method is used to create the database model for the application.
    /// </remarks>
    /// <param name="builder">
    ///     The model builder for the database context
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyProfilesConfiguration();
        builder.ApplyRoutinesConfiguration();
        builder.ApplyReservationsConfiguration();
        builder.ApplyIamConfiguration();
        builder.ApplyGymConfiguration();

        builder.UseSnakeCaseNamingConvention();
    }
}