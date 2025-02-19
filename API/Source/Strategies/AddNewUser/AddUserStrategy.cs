using API.Source.Db;
using Microsoft.AspNetCore.Identity;

namespace API.Source.Strategies.AddNewUser;

public class AddUserStrategy : IRequestStrategy<AddNewUserRequest, ResponseModelBase<object>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AddUserStrategy(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ResponseModelBase<object>> ExecuteAsync(AddNewUserRequest request)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
        };

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

    public Task ExecuteAsync(PipelineContext<AddNewUserRequest, ResponseModelBase<object>> context)
    {
        throw new NotImplementedException();
    }
}
