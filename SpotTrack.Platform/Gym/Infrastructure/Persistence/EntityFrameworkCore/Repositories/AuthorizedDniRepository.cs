using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AuthorizedDniRepository(AppDbContext context)
    : BaseRepository<AuthorizedDni>(context), IAuthorizedDniRepository
{
    public async Task<bool> ExistsByGymIdAndDniAsync(int gymId, string dni, CancellationToken cancellationToken = default)
        => await Context.Set<AuthorizedDni>()
            .AnyAsync(a => a.GymId == gymId && a.Dni == dni, cancellationToken);

    public async Task<AuthorizedDni?> FindByGymIdAndDniAsync(int gymId, string dni, CancellationToken cancellationToken = default)
        => await Context.Set<AuthorizedDni>()
            .FirstOrDefaultAsync(a => a.GymId == gymId && a.Dni == dni, cancellationToken);

    public async Task<IEnumerable<AuthorizedDni>> FindAllByGymIdAsync(int gymId, CancellationToken cancellationToken = default)
        => await Context.Set<AuthorizedDni>()
            .Where(a => a.GymId == gymId)
            .ToListAsync(cancellationToken);
}
