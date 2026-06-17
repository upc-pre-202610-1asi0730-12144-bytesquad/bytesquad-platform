using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Profiles.Domain.Model.Aggregates;

public partial class Admin
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public PersonName? Name { get; private set; }
    public ValueObjects.EmailAddress? Email { get; private set; }
    public PhoneNumber? Phone { get; private set; }
    public ValueObjects.Dni? Dni { get; private set; }

    // For EF Core
    private Admin() { }

    public Admin(RegisterAdminCommand command)
    {
        UserId = command.UserId;
    }

    public Admin(CreateAdminCommand command)
    {
        UserId = command.UserId;
        Name = new PersonName(command.FirstName, command.LastName);
        Email = new ValueObjects.EmailAddress(command.Email);
        Phone = new PhoneNumber(command.PhoneNumber);
        Dni = new ValueObjects.Dni(command.Dni);
    }

    public void UpdateProfile(UpdateAdminProfileCommand command)
    {
        Name = new PersonName(command.FirstName, command.LastName);
        Phone = new PhoneNumber(command.PhoneNumber);
    }

    public string FullName => Name?.FullName ?? string.Empty;
    public string EmailAddress => Email?.Address ?? string.Empty;
}
