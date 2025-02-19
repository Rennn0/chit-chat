using API.Source;
using API.Source.Db;
using API.Source.Factory;
using API.Source.Guards;
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

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="request">The request containing user details such as username, email, password, and roles.</param>
        /// <returns>A response model indicating the success or failure of the operation.</returns>
        /// <remarks>
        /// This endpoint requires Admin privileges.
        /// </remarks>
        [Authorize(Policy = Policies.Admin)]
        [HttpPost(nameof(AddNewUser))]
        public async Task<ResponseModelBase<object>> AddNewUser(
            [FromBody] AddNewUserRequest request
        )
        {
            return await _factory
                .GetStrategy<AddNewUserRequest, ResponseModelBase<object>>()
                .ExecuteAsync(request);
        }

        /// <summary>
        /// Authenticates a user and generates an API key.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>A response model containing the generated API key.</returns>
        [HttpPost(nameof(ApiKey))]
        public async Task<ResponseModelBase<string>> ApiKey([FromBody] LoginRequest request)
        {
            return await _factory
                .GetStrategy<LoginRequest, ResponseModelBase<string>>()
                .ExecuteAsync(request);
        }

        /// <summary>
        /// Retrieves a list of users.
        /// </summary>
        /// <returns>A response model containing a list of application users.</returns>
        [Authorize(Policy = Policies.Elevated)]
        [HttpGet(nameof(ListUsers))]
        public async Task<ResponseModelBase<IEnumerable<ApplicationUser>>> ListUsers()
        {
            return await _factory
                .GetPipeline<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>()
                .ExecuteAsync(new ListUsersRequest());
        }
    }
}
