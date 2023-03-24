using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using RailwayReservationAPI.Utility;
using System.Net;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/seat")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public SeatController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateSeat([FromBody] SeatCreateRequestDTO dto)
        {

            Seat newSeat = new()
            {
                SeatPrice = dto.SeatPrice,
                SeatStatus = SD.status_open,
                CarriageId = dto.CarriageId,
            };
            _db.Add(newSeat);
            _db.SaveChanges();
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            _response.Data = newSeat;
            return Ok(_response);
        }

    }
}
