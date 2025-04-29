using System.Reflection;
using API.Source;
using API.Source.Db;
using API.Source.Factory;
using API.Source.Guards;
using API.Source.Libs;
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
        private readonly IRequestHandlerFactory m_factory;
        private readonly UserManager<ApplicationUser> m_userManager;
        private readonly SignInManager<ApplicationUser> m_signInManager;
        private static readonly CircularList<DateTimeOffset> s_checkIns = [];
        private static readonly LinkedList<Appointment> s_appointments = [];

        public ApiController(
            IRequestHandlerFactory factory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            m_factory = factory;
            m_userManager = userManager;
            m_signInManager = signInManager;
        }

        [HttpDelete]
        public void Appointment(Guid id)
        {
            Appointment appointment = s_appointments.FirstOrDefault(a => a.Host == id.ToString());
            s_appointments.Remove(appointment);
        }

        [HttpGet("{username}")]
        public IReadOnlyCollection<Appointment> Appointments(string username) =>
            s_appointments
                .Where(a => a.Host == username)
                .OrderBy(a => a.Start)
                .ToList()
                .AsReadOnly();

        [HttpGet]
        public IReadOnlyCollection<Appointment> Appointments(
            DateTime start,
            DateTime end,
            int room
        ) =>
            s_appointments
                .Where(a => a.Start >= start && a.End <= end && a.Room == room)
                .OrderBy(a => a.Start)
                .ToList()
                .AsReadOnly();

        [HttpPost]
        public void Appointment([FromBody] Appointment appointment)
        {
            if (s_appointments.Any(a => a.Start < appointment.End && a.End > appointment.Start))
            {
                throw new InvalidOperationException("Appointment overlaps with an existing one.");
            }

            appointment = appointment with { Id = Guid.NewGuid() };
            s_appointments.AddLast(appointment);
        }

        [HttpGet]
        public void CheckIn() => s_checkIns.Add(DateTimeOffset.Now);

        [HttpGet]
        public IReadOnlyCollection<string> Checkins([FromQuery] int offsetMinutes = 0) =>
            s_checkIns
                .List.OrderByDescending(x => x)
                .Select(dto => dto.ToOffset(TimeSpan.FromMinutes(offsetMinutes)).ToString("G"))
                .ToList();

        [HttpGet]
        public string GetVersion() =>
            Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="request">The request containing user details such as username, email, password, and roles.</param>
        /// <returns>A response model indicating the success or failure of the operation.</returns>
        /// <remarks>
        /// This endpoint requires Admin privileges.
        /// </remarks>
        [Authorize(Policy = Policies.Admin, AuthenticationSchemes = "ApiKey")]
        [HttpPost]
        public async Task<ResponseModelBase<object>> ApplicationUser(
            [FromBody] AddNewUserRequest request
        )
        {
            return await m_factory
                .GetHandler<AddNewUserRequest, ResponseModelBase<object>>()
                .ExecuteAsync(request);
        }

        /// <summary>
        /// Authenticates a user and generates an API key.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>A response model containing the generated API key.</returns>
        [HttpPost]
        public async Task<ResponseModelBase<string>> Auth([FromBody] AuthRequest request)
        {
            return await m_factory
                .GetPipeline<AuthRequest, ResponseModelBase<string>>()
                .ExecuteAsync(request);
        }

        /// <summary>
        /// Retrieves a list of users.
        /// </summary>
        /// <returns>A response model containing a list of application users.</returns>
        [Authorize(AuthenticationSchemes = "ApiKey,Bearer")]
        [HttpGet]
        public async Task<ResponseModelBase<IEnumerable<ApplicationUser>>> ApplicationUsers()
        {
            return await m_factory
                .GetPipeline<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>()
                .ExecuteAsync(new ListUsersRequest());
        }

        [Authorize(Policy = Policies.Admin, AuthenticationSchemes = "ApiKey")]
        [Authorize(Policy = Policies.Moderator, AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ResponseModelBase<AddNewTenantResponse>> Tenant(
            [FromBody] AddNewTenantRequest request
        )
        {
            return await m_factory
                .GetPipeline<AddNewTenantRequest, ResponseModelBase<AddNewTenantResponse>>()
                .ExecuteAsync(request);
        }

        [Authorize(Policy = Policies.Public, AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ResponseModelBase<IEnumerable<ListTenantsResponse>>> Tenants(
            [FromQuery] ListTenantsRequest request
        )
        {
            return await m_factory
                .GetPipeline<
                    ListTenantsRequest,
                    ResponseModelBase<IEnumerable<ListTenantsResponse>>
                >()
                .ExecuteAsync(request);
        }

        /// <summary>
        /// Initiates the Google authentication process.
        /// </summary>
        /// <param name="redirectUri">The URI to redirect to after authentication.</param>
        /// <returns>An authentication challenge result.</returns>
        [HttpGet]
        public IActionResult GoogleAuth(string redirectUri)
        {
            AuthenticationProperties properties =
                m_signInManager.ConfigureExternalAuthenticationProperties(
                    provider: GoogleDefaults.AuthenticationScheme,
                    Url.Action("GoogleRedirect", "Api")
                );

            properties.Items["redirect_uri"] = redirectUri;
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles the Google authentication redirect.
        /// </summary>
        /// <returns>A redirection to the specified URI with a token or error message.</returns>
        [HttpGet]
        public async Task<IActionResult> GoogleRedirect()
        {
            AuthenticateResult authResult = await HttpContext.AuthenticateAsync(
                GoogleDefaults.AuthenticationScheme
            );

            if (
                authResult.Properties is null
                || !authResult.Properties.Items.TryGetValue("redirect_uri", out string? redirectUri)
            )
            {
                return BadRequest(error: "missing redirect_uri");
            }

            ResponseModelBase<string> result = await m_factory
                .GetPipeline<AuthRequest.GoogleRedirect, ResponseModelBase<string>>()
                .ExecuteAsync(new AuthRequest.GoogleRedirect(), HttpContext);

            return result.Success
                ? Redirect($"{redirectUri}?token={result.Data}")
                : Redirect($"{redirectUri}?error={result.Error}");
        }
    }
}
