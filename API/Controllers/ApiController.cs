using API.Source.Guard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly TokenManager _tokenManager;

        public ApiController(TokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [Authorize(Policy = Policies.AdminRolePolicy)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Admin");
        }
    }
}
