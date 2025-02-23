using API.Source.Db;
using API.Source.Guards;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace API.Source.Handlers.Authorization
{
    public class TwoFactorHandler : IRequestHandler<AuthRequest, ResponseModelBase<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenManager _tokenManager;
        private readonly IMemoryCache _cache;

        public TwoFactorHandler(
            UserManager<ApplicationUser> userManager,
            TokenManager tokenManager,
            IMemoryCache cache
        )
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
            _cache = cache;
        }

        public async Task<ResponseModelBase<string>> ExecuteAsync(AuthRequest request)
        {
            ResponseModelBase<string> response = new ResponseModelBase<string>();
            ApplicationUser? user = await _userManager.FindByNameAsync(request.Username);

            if (user is null)
            {
                response.Success = false;
                response.Error = "User not found";
                return response;
            }

            if (
                !await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.TwoFactorToken)
            )
            {
                response.Success = false;
                response.Error = "Invalid 2FA Token";
                return response;
            }

            if (!_cache.TryGet2FaToken(user.Id, out _))
            {
                response.Success = false;
                response.Error = "Used or Expired 2FA Token";
                return response;
            }

            _cache.Remove2FaToken(user.Id);
            response.Data = await _tokenManager.GenerateJwtTokenAsync(user, _userManager);
            return response;
        }

        public async Task ExecuteAsync(
            PipelineContext<AuthRequest, ResponseModelBase<string>> context
        )
        {
            if (context.Request.Method != AuthRequest.AUTH_METHOD.Verify2Fa)
            {
                return;
            }

            context.Response = await ExecuteAsync(context.Request);
        }
    }
}
