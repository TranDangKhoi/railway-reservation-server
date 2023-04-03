using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("{carriageId}")]
        public async Task<ActionResult<ApiResponse>> GetSeatsByCarriageId(int carriageId)
        {
            var foundSeatsFromDb = _db.Seats.Where(c => c.CarriageId == carriageId);
            if (foundSeatsFromDb == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = "Không tìm thấy toa tàu";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = foundSeatsFromDb;
            return Ok(_response);
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

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateSeatStatus([FromBody] SeatUpdateDTO dto)
        {
            Seat foundSeatFromDb = _db.Seats.FirstOrDefault(u => u.Id == dto.Id);
            foundSeatFromDb.SeatStatus = dto.SeatStatus;
            _db.Update(foundSeatFromDb);
            _db.SaveChanges();
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = foundSeatFromDb;
            return Ok(_response);
        }

    }
}
