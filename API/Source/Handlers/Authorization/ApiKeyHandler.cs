using API.Source.Db;
using API.Source.Guards;
using Microsoft.AspNetCore.Identity;

namespace API.Source.Handlers.Login
{
    public class ApiKeyHandler : IRequestHandler<AuthRequest, ResponseModelBase<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenManager _tokenManager;

        public ApiKeyHandler(UserManager<ApplicationUser> userManager, TokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        public async Task<ResponseModelBase<string>> ExecuteAsync(AuthRequest request)
        {
            ResponseModelBase<string> response = new ResponseModelBase<string>();
            ApplicationUser? user = await _userManager.FindByNameAsync(request.Username);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                response.Success = false;
                response.Error = "Invalid Credentials";
                return response;
            }

            if (request.CreateKeyIfNotExists && string.IsNullOrEmpty(user.ApiKey))
            {
                response.Data = await _tokenManager.GenerateApiKey(user, _userManager);
                return response;
            }

            if (user.ApiKeyExpiry > DateTime.UtcNow && !string.IsNullOrEmpty(user.ApiKey))
            {
                response.Data = user.ApiKey;
                return response;
            }

            response.Success = false;
            response.Error = "No valid key found";
            return response;
        }

        public async Task ExecuteAsync(PipelineContext<AuthRequest, ResponseModelBase<string>> context)
        {
            if (context.Request.Method != AuthRequest.AuthMethod.ApiKey)
            {
                return;
            }

            context.Response = await ExecuteAsync(context.Request);
        }
    }
}