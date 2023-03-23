using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using System.Net;
using System.Runtime.CompilerServices;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/track")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public TrackController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllTracksAndTrains()
        {
            var trainTracks = _db.Tracks
                .Include(u => u.Train)
                .ThenInclude(u => u.Carriages)
                .ThenInclude(u => u.Seats)
                .Include(u => u.Train)
                .ThenInclude(u => u.Carriages)
                .ThenInclude(u => u.CarriageType);
            if (trainTracks == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = "Không tìm thấy chuyến đi nào";
                return NotFound(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = trainTracks;
            return Ok(trainTracks);
        }

        [HttpGet("find")]
        // Tìm ra chuyến tàu dựa theo các tham số được truyền vào như sau:
        // ga đi, ga đến, thời gian khởi hành, thời gian kết thúc
        public async Task<ActionResult<ApiResponse>> FindTrack(string departureStation, DateTime departureTime, string arrivalStation, DateTime returnTime)
        {
            // Bắt đầu tìm thôi
            var track = _db.Tracks
                .Include(u => u.Train)
                .ThenInclude(u => u.Carriages)
                .ThenInclude(u => u.Seats)
                .Include(u => u.Train)
                .ThenInclude(u => u.Carriages)
                .ThenInclude(u => u.CarriageType)
                .Where(u => u.DepartureStation == departureStation)
                .Where(u => u.DepartureTime >= departureTime && u.ReturnTime <= returnTime)
                .Where(u => u.ArrivalStation == arrivalStation);
            if (track == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = "Không tìm thấy chuyến đi nào phù hợp với yêu cầu của bạn";
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = track;
            return Ok(track);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateNewTrainTracks([FromBody] TrackCreateRequestDTO dto)
        {
            Track existedTrainTracks = _db.Tracks
                .Include(u => u.Train)
                .FirstOrDefault(u => u.DepartureStation == dto.DepartureStation && u.DepartureTime == dto.DepartureTime && u.ReturnTime == dto.ReturnTime && u.ArrivalStation == dto.ArrivalStation);
            if (existedTrainTracks != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = "Chuyến đi bạn muốn tạo đã tồn tại";
                return BadRequest(_response);
            }

            Train train = _db.Trains.FirstOrDefault(u => u.Name == dto.TrainName);

            if (train == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = "Không tìm được tàu dựa theo tên bạn cung cấp";
                _response.StatusCode = HttpStatusCode.NotFound;
                return BadRequest(_response);
            }

            Track newTrack = new()
            {
                DepartureStation = dto.DepartureStation,
                ArrivalStation = dto.ArrivalStation,
                ReturnTime = dto.ReturnTime,
                DepartureTime = dto.DepartureTime,
                Train = train,
            };
            _db.Add(newTrack);
            _db.SaveChanges();
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = newTrack;
            return Ok(_response);
        }
    }
}
