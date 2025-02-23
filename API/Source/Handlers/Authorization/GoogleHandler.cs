using System.Security.Claims;
using API.Source.Db;
using API.Source.Guards;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;

namespace API.Source.Handlers.Authorization;

public class GoogleHandler : IRequestHandler<AuthRequest.GoogleRedirect, ResponseModelBase<string>>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenManager _tokenManager;

    public GoogleHandler(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        TokenManager tokenManager
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenManager = tokenManager;
    }

    public Task<ResponseModelBase<string>> ExecuteAsync(AuthRequest.GoogleRedirect request)
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(
        PipelineContext<AuthRequest.GoogleRedirect, ResponseModelBase<string>> context
    )
    {
        if (context.HttpContext is null)
        {
            return;
        }

        context.Response = new ResponseModelBase<string>();

        AuthenticateResult authResult = await context.HttpContext.AuthenticateAsync(
            GoogleDefaults.AuthenticationScheme
        );

        if (!authResult.Succeeded || !authResult.Principal.Identities.Any())
        {
            context.Response.Success = false;
            return;
        }

        IEnumerable<Claim> claims = authResult.Principal.Identities.ElementAt(0).Claims;

        Claim? emailClaim = authResult.Principal.FindFirst(ClaimTypes.Email);
        Claim? nameClaim = authResult.Principal.FindFirst(ClaimTypes.Name);

        if (emailClaim is null || nameClaim is null)
        {
            context.Response.Success = false;
            return;
        }

        ApplicationUser? user = await _userManager.FindByEmailAsync(emailClaim.Value);

        if (user is null)
        {
            user = new ApplicationUser
            {
                Email = emailClaim.Value,
                UserName = nameClaim.Value,
                EmailConfirmed = true,
            };
            await _userManager.CreateAsync(user);
        }

        ExternalLoginInfo? externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

        if (externalLoginInfo is null)
        {
            context.Response.Success = false;
            return;
        }

        await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        await context.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        await _userManager.AddLoginAsync(user, externalLoginInfo);

        context.Response.Data = await _tokenManager.GenerateJwtTokenAsync(user, _userManager);
    }
}
