using API.Source;
using API.Source.Db;
using API.Source.Guards;
using API.Source.Handlers;
using Microsoft.AspNetCore.Identity;

namespace API.Source.Handlers.Login
{
    public class TokenHandler : IRequestHandler<LoginRequest, ResponseModelBase<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenManager _tokenManager;

        public TokenHandler(UserManager<ApplicationUser> userManager, TokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
        }
        public async Task<ResponseModelBase<string>> ExecuteAsync(LoginRequest request)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(request.Username);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new ResponseModelBase<string>()
                {
                    Success = false,
                    Error = "Invalid Credentials",
                };
            }

            string token = await _tokenManager.GenerateJwtToken(user, _userManager) ?? string.Empty;
            return new ResponseModelBase<string>() { Data = token };
        }

        public async Task ExecuteAsync(PipelineContext<LoginRequest, ResponseModelBase<string>> context)
        {
            if (context.Request.Method != LoginRequest.AuthMethod.Token)
            {
                return;
            }

            context.Response = await ExecuteAsync(context.Request);
        }
    }
}