namespace SpotTrack.Platform.Gyms.Domain.Model.Entities;

public class AuthorizedDni
{
    public int Id { get; private set; }
    public int GymId { get; private set; }
    public string Dni { get; private set; } = null!;

    private AuthorizedDni() { }

    public AuthorizedDni(int gymId, string dni)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dni);
        if (dni.Length != 8 || !dni.All(char.IsDigit))
            throw new ArgumentException("DNI must be exactly 8 digits.", nameof(dni));

        GymId = gymId;
        Dni = dni;
    }
}
