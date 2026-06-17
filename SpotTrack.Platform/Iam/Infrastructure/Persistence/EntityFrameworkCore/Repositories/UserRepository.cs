using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserRepository(AppDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        await Context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        await Context.Set<User>()
            .AnyAsync(u => u.Username == username, cancellationToken);
}
