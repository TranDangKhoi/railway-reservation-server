using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/authtest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet("test")]
        [Authorize]
        public async Task<ActionResult<string>> Test()
        {
            return "You are authenticated";
        }
    }
}
