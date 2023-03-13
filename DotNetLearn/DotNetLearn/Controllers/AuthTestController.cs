using DotNetLearn.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet("auth-customertest")]
        [Authorize]
        public async Task<ActionResult<string>> GetAuthenticated()
        {
            return "Mày authen rồi đấy";
        }

        [HttpGet("auth-admintest")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<string>> GetAuthenticatedAdmin()
        {
            return "Mày chính là Admin";
        }
    }
}
