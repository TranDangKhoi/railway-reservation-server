using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/seat")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateSeat([FromBody] SeatCreateRequestDTO dto)
        {

        }
    }
}
