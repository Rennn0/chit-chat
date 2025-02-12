using API.Source;
using API.Source.Db;
using API.Source.Factory;
using API.Source.Guard;
using llibrary.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IRequestHandlerFactory _factory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<IWarningLogger> _warning;

        public ApiController(
            IRequestHandlerFactory factory,
            IMemoryCache cache,
            ILogger<IWarningLogger> warning
        )
        {
            _factory = factory;
            _cache = cache;
            _warning = warning;
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpPost(nameof(AddNewUser))]
        public async Task<ResponseModelBase<object>> AddNewUser(
            [FromBody] AddNewUserRequest request
        )
        {
            return await _factory
                .GetHandler<AddNewUserRequest, ResponseModelBase<object>>()
                .HandleAsync(request);
        }

        [HttpPost(nameof(ApiKey))]
        public async Task<ResponseModelBase<string>> ApiKey([FromBody] LoginRequest request)
        {
            return await _factory
                .GetHandler<LoginRequest, ResponseModelBase<string>>()
                .HandleAsync(request);
        }

        [Authorize(Policy = Policies.Elevated)]
        [HttpGet(nameof(ListUsers))]
        public async Task<ResponseModelBase<IEnumerable<ApplicationUser>>?> ListUsers()
        {
            ListUsersRequest request = new();
            return await _cache.GetOrCreateAsync(
                key: request.GetHashCode(),
                factory: async f =>
                {
                    f.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(1);
                    return await _factory
                        .GetHandler<
                            ListUsersRequest,
                            ResponseModelBase<IEnumerable<ApplicationUser>>
                        >()
                        .HandleAsync(request);
                }
            );
        }
    }
}
