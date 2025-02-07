using API.Source;
using API.Source.Factory;
using API.Source.Guard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IRequestHandlerFactory _factory;

        public ApiController(IRequestHandlerFactory factory)
        {
            _factory = factory;
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
    }
}
