using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SpotTrack.Platform.Memberships.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PaymentRepository(AppDbContext context) : IPaymentRepository
{
    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default) =>
        await context.Set<Payment>().AddAsync(payment, cancellationToken);

    public async Task<Payment?> FindByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default) =>
        await context.Set<Payment>()
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId, cancellationToken);

    public void Update(Payment payment) =>
        context.Set<Payment>().Update(payment);
}
