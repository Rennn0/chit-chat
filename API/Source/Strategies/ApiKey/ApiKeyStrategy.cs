using API.Source.Db;
using API.Source.Guards;
using Microsoft.AspNetCore.Identity;

namespace API.Source.Strategies.ApiKey;

public class ApiKeyStrategy : IRequestStrategy<LoginRequest, ResponseModelBase<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenManager _tokenManager;

    public ApiKeyStrategy(UserManager<ApplicationUser> userManager, TokenManager tokenManager)
    {
        _userManager = userManager;
        _tokenManager = tokenManager;
    }

    public async Task<ResponseModelBase<string>> ExecuteAsync(LoginRequest request)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new ResponseModelBase<string>()
            {
                Success = false,
                Error = "Invalid Credentials",
            };
        }

        //string token = await _tokenManager.GenerateJwtToken(user, _userManager);
        string token = await _tokenManager.GenerateApiKey(user, _userManager);
        return new ResponseModelBase<string>() { Data = token };
    }

    public Task ExecuteAsync(PipelineContext<LoginRequest, ResponseModelBase<string>> context)
    {
        Console.WriteLine(context.Request.Password);
        return Task.CompletedTask;
    }
}
