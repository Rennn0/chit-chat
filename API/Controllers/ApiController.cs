using System.Security.Claims;
using API.Source;
using API.Source.Db;
using API.Source.Factory;
using API.Source.Guards;
using llibrary.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IRequestHandlerFactory _factory;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ApiController(
            IRequestHandlerFactory factory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _factory = factory;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="request">The request containing user details such as username, email, password, and roles.</param>
        /// <returns>A response model indicating the success or failure of the operation.</returns>
        /// <remarks>
        /// This endpoint requires Admin privileges.
        /// </remarks>
        [Authorize(Policy = Policies.Admin, AuthenticationSchemes = "ApiKey")]
        [HttpPost(template: "user")]
        public async Task<ResponseModelBase<object>> AddNewUser(
            [FromBody] AddNewUserRequest request
        )
        {
            return await _factory
                .GetHandler<AddNewUserRequest, ResponseModelBase<object>>()
                .ExecuteAsync(request);
        }

        /// <summary>
        /// Authenticates a user and generates an API key.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>A response model containing the generated API key.</returns>
        [HttpPost(template: "auth")]
        public async Task<ResponseModelBase<string>> Auth([FromBody] AuthRequest request)
        {
            return await _factory
                .GetPipeline<AuthRequest, ResponseModelBase<string>>()
                .ExecuteAsync(request);
        }

        /// <summary>
        /// Retrieves a list of users.
        /// </summary>
        /// <returns>A response model containing a list of application users.</returns>
        [Authorize(AuthenticationSchemes = "ApiKey,Bearer")]
        [HttpGet(template: "users")]
        public async Task<ResponseModelBase<IEnumerable<ApplicationUser>>> ListUsers()
        {
            return await _factory
                .GetPipeline<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>()
                .ExecuteAsync(new ListUsersRequest());
        }

        [Authorize(Policy = Policies.Admin, AuthenticationSchemes = "ApiKey")]
        [Authorize(Policy = Policies.Moderator, AuthenticationSchemes = "Bearer")]
        [HttpPost(template: "tenant")]
        public async Task<ResponseModelBase<AddNewTenantResponse>> AddNewTenant(
            [FromBody] AddNewTenantRequest request
        )
        {
            return await _factory
                .GetPipeline<AddNewTenantRequest, ResponseModelBase<AddNewTenantResponse>>()
                .ExecuteAsync(request);
        }

        [Authorize(Policy = Policies.Admin, AuthenticationSchemes = "ApiKey")]
        [Authorize(Policy = Policies.Moderator, AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = Policies.Elevated, AuthenticationSchemes = "Bearer")]
        [HttpGet(template: "tenants")]
        public async Task<ResponseModelBase<IEnumerable<ListTenantsResponse>>> AddNewTenant(
            [FromQuery] ListTenantsRequest request
        )
        {
            return await _factory
                .GetPipeline<
                    ListTenantsRequest,
                    ResponseModelBase<IEnumerable<ListTenantsResponse>>
                >()
                .ExecuteAsync(request);
        }

        [HttpGet(template: "login")]
        public IActionResult Login(ILogger<IWarningLogger> logger)
        {
            var redirectUrl = Url.Action("GoogleResponse", "Api", null, Request.Scheme);

            logger.LogWarning(redirectUrl);

            var properties = signInManager.ConfigureExternalAuthenticationProperties(
                GoogleDefaults.AuthenticationScheme,
                redirectUrl
            );
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet(template: "signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authResult = await HttpContext.AuthenticateAsync(
                GoogleDefaults.AuthenticationScheme
            );
            if (!authResult.Succeeded)
            {
                return BadRequest();
            }

            var claims = authResult.Principal.Identities.FirstOrDefault()?.Claims;
            var email =
                authResult.Principal.FindFirst(ClaimTypes.Email)?.Value
                ?? throw new Exception("Email not found");
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new ApplicationUser { Email = email, UserName = email };
                await userManager.CreateAsync(user);
            }

            var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            await userManager.AddLoginAsync(user, externalLoginInfo);
            return Ok(new { Claims = claims });
        }
    }
}
