using System.Security.Cryptography;
using API.Source;
using API.Source.Factory;
using API.Source.Guard;
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

        public ApiController(IRequestHandlerFactory factory, IMemoryCache cache)
        {
            _factory = factory;
            _cache = cache;
        }

        [Authorize(Policy = Policies.AdminRolePolicy)]
        [HttpPost(nameof(AddNewUser))]
        public async Task<ResponseModelBase<object>> AddNewUser(
            [FromBody] AddNewUserRequest request
        )
        {
            return await _factory
                .GetHandler<AddNewUserRequest, ResponseModelBase<object>>()
                .HandleAsync(request);
        }

        [HttpPost(nameof(Login))]
        public async Task<ResponseModelBase<string>> Login([FromBody] LoginRequest request)
        {
            return await _factory
                .GetHandler<LoginRequest, ResponseModelBase<string>>()
                .HandleAsync(request);
        }

        [HttpGet]
        public ActionResult Cache()
        {
            string? cached = _cache.GetOrCreate(
                "user",
                entry =>
                {
                    entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10);

                    byte[] bytes = new byte[16];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(bytes);
                    }

                    return Convert.ToBase64String(bytes);
                }
            );

            return Ok(cached);
        }
    }
}
