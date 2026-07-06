using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Iam.Domain.Model.Aggregates;

public class PendingRegistration
{
    private PendingRegistration() { }

    public PendingRegistration(SavePendingRegistrationCommand command, string hashedPassword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Email);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.FirstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.LastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.PhoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Dni);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.CompanyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Ruc);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.LegalStructure);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.StreetAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.City);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.District);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.MembershipTier);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword);

        RegistrationId = Guid.NewGuid();
        Email = command.Email;
        HashedPassword = hashedPassword;
        FirstName = command.FirstName;
        LastName = command.LastName;
        PhoneNumber = command.PhoneNumber;
        Dni = command.Dni;
        CompanyName = command.CompanyName;
        Ruc = command.Ruc;
        LegalStructure = command.LegalStructure;
        CompanyPhone = command.CompanyPhone;
        CompanyEmail = command.CompanyEmail;
        StreetAddress = command.StreetAddress;
        City = command.City;
        District = command.District;
        MembershipTier = command.MembershipTier;
        CreatedAt = DateTimeOffset.UtcNow;
        ExpiresAt = CreatedAt.AddMinutes(30);
        Status = PendingRegistrationStatus.Pending;
    }

    public Guid RegistrationId { get; private set; }
    public string Email { get; private set; } = null!;
    public string HashedPassword { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string Dni { get; private set; } = null!;
    public string CompanyName { get; private set; } = null!;
    public string Ruc { get; private set; } = null!;
    public string LegalStructure { get; private set; } = null!;
    public string CompanyPhone { get; private set; } = null!;
    public string CompanyEmail { get; private set; } = null!;
    public string StreetAddress { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string District { get; private set; } = null!;
    public string MembershipTier { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public PendingRegistrationStatus Status { get; private set; }

    public bool IsExpired() => DateTimeOffset.UtcNow >= ExpiresAt;
    public bool IsPending() => Status == PendingRegistrationStatus.Pending;

    public void Consume()
    {
        if (Status == PendingRegistrationStatus.Consumed) return;
        Status = PendingRegistrationStatus.Consumed;
    }
}
