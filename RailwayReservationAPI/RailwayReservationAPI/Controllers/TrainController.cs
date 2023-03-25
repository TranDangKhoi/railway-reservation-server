using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using System.Diagnostics;
using System.Net;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/train")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public TrainController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllTrains()
        {
           var trainList = _db.Trains
                .Include(u => u.Carriages).ThenInclude(u => u.Seats)
                .Include(u => u.Carriages).ThenInclude(u => u.CarriageType)
                .ToList();
           if(trainList == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = "Không có tàu nào tồn tại";
                return NotFound(_response);
            }
           
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = trainList;
            return Ok(_response);
        }

        [HttpGet("trainId")]
        public async Task<ActionResult<ApiResponse>> GetTrainById(int trainId)
        {
            Train foundTrainFromDb = _db.Trains
                .Include(u => u.Carriages).ThenInclude(u => u.Seats)
                .Include(u => u.Carriages).ThenInclude(u => u.CarriageType).FirstOrDefault(u => u.Id == trainId);
            if(foundTrainFromDb == null) {
                return NotFound();
            }
            int totalSeats = foundTrainFromDb.Carriages.Sum(c => c.Seats.Count);
            int reversedSeats = foundTrainFromDb.Carriages.SelectMany(c => c.Seats).Count(s => s.SeatStatus == 0);
            int freeSeats = totalSeats - reversedSeats;
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateTrain(int carriageId, [FromBody] TrainCreateRequestDTO dto)
        {
            Train existedTrain = _db.Trains.FirstOrDefault(u => u.Name == dto.Name);
            if (existedTrain != null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = "Tên tàu đã tồn tại, vui lòng chọn một cái tên khác";
                return BadRequest(_response);
            }
            Train newTrain = new()
            {
                Name = dto.Name,
            };
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            _response.Data = newTrain;
            _db.Add(newTrain);
            _db.SaveChanges();
            return Ok(_response);
        }
    }
}
