using System.Security.Claims;
using System.Text.Encodings.Web;
using API.Source.Db;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace API.Source.Guards;

public class ApiKeyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApiKeyAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        UserManager<ApplicationUser> userManager
    )
        : base(options, logger, encoder)
    {
        _userManager = userManager;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-API-KEY", out StringValues apiKeyHeaderValue))
        {
            return AuthenticateResult.NoResult();
        }

        string? apiKey = apiKeyHeaderValue.FirstOrDefault();
        if (string.IsNullOrEmpty(apiKey))
        {
            return AuthenticateResult.NoResult();
        }

        ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.ApiKey == apiKey && x.ApiKeyExpiry > DateTime.UtcNow
        );

        if (user == null)
        {
            return AuthenticateResult.Fail("Invalid API key");
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            .. roles.Select(role => new Claim(ClaimTypes.Role, role)),
        ];

        ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}