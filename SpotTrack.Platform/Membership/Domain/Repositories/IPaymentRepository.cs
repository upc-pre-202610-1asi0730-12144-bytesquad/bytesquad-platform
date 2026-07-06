using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Memberships.Domain.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment?> FindByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default);
    void Update(Payment payment);
}
