using SpotTrack.Platform.Gym.Domain.Model.Aggregates;
using SpotTrack.Platform.Gym.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Gym.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class GymRepository(AppDbContext context) : BaseRepository<Gym>(context), IGymRepository
{
}
