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

    public string? PasswordResetCodeHash { get; private set; }
    public DateTimeOffset? PasswordResetExpiresAt { get; private set; }

    public void UpdateUsername(string username) => Username = username;
    public void UpdatePasswordHash(string passwordHash) => PasswordHash = passwordHash;

    public void SetResetCode(string hashedCode, DateTimeOffset expiresAt)
    {
        PasswordResetCodeHash = hashedCode;
        PasswordResetExpiresAt = expiresAt;
    }

    public void ClearResetCode()
    {
        PasswordResetCodeHash = null;
        PasswordResetExpiresAt = null;
    }
}
