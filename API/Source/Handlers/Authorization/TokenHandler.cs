using API.Source.Db;
using API.Source.Guards;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Resend;

namespace API.Source.Handlers.Authorization
{
    public class TokenHandler : IRequestHandler<AuthRequest, ResponseModelBase<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenManager _tokenManager;
        private readonly IResend _mailer;
        private readonly IMemoryCache _cache;

        public TokenHandler(
            UserManager<ApplicationUser> userManager,
            TokenManager tokenManager,
            IResend mailer,
            IMemoryCache cache
        )
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
            _mailer = mailer;
            _cache = cache;
        }

        public async Task<ResponseModelBase<string>> ExecuteAsync(AuthRequest request)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(request.Username);
            ResponseModelBase<string> response = new ResponseModelBase<string>();

            if (
                user is null
                || user.Email is null
                || !await _userManager.CheckPasswordAsync(user, request.Password)
            )
            {
                response.Success = false;
                response.Error = "Invalid Credentials";
                return response;
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                string tft = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                await _mailer.SendTwoFactorToken(user.Email, tft);
                _cache.Set2FaToken(user.Id, tft);

                response.Data = "Two Factor Authentication Code Sent";
                return response;
            }

            string token = await _tokenManager.GenerateJwtTokenAsync(user, _userManager);
            return new ResponseModelBase<string>() { Data = token };
        }

        public async Task ExecuteAsync(
            PipelineContext<AuthRequest, ResponseModelBase<string>> context
        )
        {
            if (context.Request.Method != AuthRequest.AUTH_METHOD.Token)
            {
                return;
            }

            context.Response = await ExecuteAsync(context.Request);
        }
    }
}
