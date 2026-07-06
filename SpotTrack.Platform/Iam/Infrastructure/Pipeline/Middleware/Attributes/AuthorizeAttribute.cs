using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute
{
    public UserRole? Role { get; }
    public AuthorizeAttribute() { }
    public AuthorizeAttribute(UserRole role) { Role = role; }
}
