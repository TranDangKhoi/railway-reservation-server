using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
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
           var trainList = _db.Trains.ToList();
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateTrain([FromBody] TrainCreateRequestDTO dto)
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
            if (ModelState.IsValid)
            {
                _db.Add(newTrain);
                _db.SaveChanges();
                foreach (var carriageDto in dto.CarriageCreateRequestDTOs)
                {
                    Carriage carriages = new()
                    {
                        TrainId = newTrain.Id,
                        CarriageNo = carriageDto.CarriageNo,
                        TotalSeats = carriageDto.TotalSeats,
                    };
                    _db.Carriages.Add(carriages);
                    _db.SaveChanges();
                }
                if (ModelState.IsValid)
                {
                    foreach(var carriageTypeDto in dto.CarriageTypeCreateRequestDTOs)
                    {
                        CarriageType carriageTypes = new()
                        {
                            Name = carriageTypeDto.CarriageTypeName
                        };
                    }         
                }
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            _response.Data = newTrain;
            return Ok(_response);
        }
    }
}
