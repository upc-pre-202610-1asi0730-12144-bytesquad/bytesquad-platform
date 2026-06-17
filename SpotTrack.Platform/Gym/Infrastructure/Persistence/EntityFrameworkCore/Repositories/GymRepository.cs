using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class GymRepository(AppDbContext context) : BaseRepository<Gym>(context), IGymRepository
{
}
