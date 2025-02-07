using API.Source.Guard;
using Microsoft.AspNetCore.Identity;

namespace API.Source.Strategy;

public interface IRequestHandlerStrategy<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}

public class AddUserStrategy : IRequestHandlerStrategy<AddNewUserRequest, ResponseModelBase<object>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AddUserStrategy(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ResponseModelBase<object>> HandleAsync(AddNewUserRequest request)
    {
        IdentityUser user = new IdentityUser { UserName = request.Username, Email = request.Email };

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new ResponseModelBase<object>() { Success = false, Error = result.Errors };
        }

        foreach (string role in request.Roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);
        }

        return new ResponseModelBase<object>();
    }
}

public class LoginStrategy : IRequestHandlerStrategy<LoginRequest, ResponseModelBase<string>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly TokenManager _tokenManager;

    public LoginStrategy(UserManager<IdentityUser> userManager, TokenManager tokenManager)
    {
        _userManager = userManager;
        _tokenManager = tokenManager;
    }

    public async Task<ResponseModelBase<string>> HandleAsync(LoginRequest request)
    {
        IdentityUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new ResponseModelBase<string>()
            {
                Success = false,
                Error = "Invalid Credentials",
            };
        }

        string token = _tokenManager.GenerateJwtToken(user, _userManager);
        return new ResponseModelBase<string>() { Data = token };
    }
}
