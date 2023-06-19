using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapingway.Application.Abstractions;
using Mapingway.Common.Permission;
using Mapingway.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;


    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }


    public string GenerateToken(User user, IEnumerable<Permissions> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permissions", permission.ToString()));
        }

        var signingKey = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey)),
        SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(10),
            signingKey);

        var result = new JwtSecurityTokenHandler().WriteToken(token);

        return result;
    }
}