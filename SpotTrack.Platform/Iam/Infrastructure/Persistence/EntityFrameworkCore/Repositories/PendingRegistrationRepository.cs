using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SpotTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PendingRegistrationRepository(AppDbContext context) : IPendingRegistrationRepository
{
    public async Task AddAsync(PendingRegistration registration, CancellationToken cancellationToken = default) =>
        await context.Set<PendingRegistration>().AddAsync(registration, cancellationToken);

    public async Task<PendingRegistration?> FindByRegistrationIdAsync(Guid registrationId, CancellationToken cancellationToken = default) =>
        await context.Set<PendingRegistration>()
            .FirstOrDefaultAsync(r => r.RegistrationId == registrationId, cancellationToken);

    public void Update(PendingRegistration registration) =>
        context.Set<PendingRegistration>().Update(registration);
}
