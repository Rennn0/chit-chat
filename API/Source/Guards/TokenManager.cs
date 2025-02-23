using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Source.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Source.Guards;

public class TokenManager
{
    private readonly IConfiguration _config;

    public TokenManager(IConfiguration configuration)
    {
        _config = configuration;
    }

    public async Task<string> GenerateJwtTokenAsync(
        ApplicationUser user,
        UserManager<ApplicationUser> userManager
    )
    {
        List<Claim> claims = [new Claim(JwtRegisteredClaimNames.Sub, user.Id)];

        IList<string> roles = await userManager.GetRolesAsync(user);
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

    public async Task<string?> GenerateApiKeyAsync(
        ApplicationUser user,
        UserManager<ApplicationUser> userManager
    )
    {
        string? key = Guid.NewGuid().ToString("N");
        user.ApiKey = key;
        user.ApiKeyExpiry = DateTime.UtcNow.AddYears(1);

        await userManager.UpdateAsync(user);
        return key;
    }
}
