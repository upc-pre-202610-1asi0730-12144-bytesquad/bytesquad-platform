using SpotTrack.Platform.Maintenances.Domain.Model.Commands;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;

public class Technician
{
    private Technician() { }

    public Technician(CreateTechnicianCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name is required.", nameof(command.Name));
        if (string.IsNullOrWhiteSpace(command.Specialization))
            throw new ArgumentException("Specialization is required.", nameof(command.Specialization));
        if (command.AdminId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.AdminId), "AdminId must be a positive integer.");

        Name = command.Name.Trim();
        Specialization = command.Specialization.Trim();
        PhoneNumber = command.PhoneNumber?.Trim();
        AdminId = command.AdminId;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Specialization { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public int AdminId { get; private set; }
}
