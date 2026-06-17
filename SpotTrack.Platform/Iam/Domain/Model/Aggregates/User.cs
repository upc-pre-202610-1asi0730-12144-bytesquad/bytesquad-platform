using System.Text.Json.Serialization;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Iam.Domain.Model.Aggregates;

public partial class User
{
    public int Id { get; private set; }
    public string Username { get; private set; } = null!;
    [JsonIgnore]
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }

    private User() { }

    public User(string username, string passwordHash, UserRole role)
    {
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void UpdateUsername(string username) => Username = username;
    public void UpdatePasswordHash(string passwordHash) => PasswordHash = passwordHash;
}
