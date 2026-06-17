using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SpotTrack.Platform.Iam.Application.Internal.OutboundServices;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;

namespace SpotTrack.Platform.Iam.Infrastructure.Tokens.Jwt.Services;

public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
{
    private readonly string _secret = tokenSettings.Value.Secret;

    public string GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(tokenDescriptor);
    }

    public async Task<int?> ValidateToken(string token)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(_secret);
            var handler = new JsonWebTokenHandler();
            var result = await handler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

            if (!result.IsValid) return null;

            var sidClaim = result.ClaimsIdentity.FindFirst(ClaimTypes.Sid);
            return sidClaim is not null ? int.Parse(sidClaim.Value) : null;
        }
        catch
        {
            return null;
        }
    }
}
