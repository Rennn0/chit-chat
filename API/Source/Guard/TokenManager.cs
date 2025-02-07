using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Source.Guard;

public class TokenManager
{
    private readonly IConfiguration _config;

    public TokenManager(IConfiguration configuration)
    {
        _config = configuration;
    }

    public string GenerateJwtToken(IdentityUser user, UserManager<IdentityUser> userManager)
    {
        Debug.Assert(user.Email != null, "user.Email != null");

        List<Claim> claims = [new Claim(JwtRegisteredClaimNames.Sub, user.Id)];

        IList<string> roles = userManager.GetRolesAsync(user).Result;
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new Exception("Jwt key required"))
        );
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(int.Parse(_config["Jwt:TTL"] ?? "1")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
