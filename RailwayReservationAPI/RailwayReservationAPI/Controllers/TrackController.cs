using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using System;
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
            var tracks = _db.Tracks
                .Include(u => u.Train).ThenInclude(u => u.Carriages).ThenInclude(u => u.Seats)
                .Include(u => u.Train).ThenInclude(u => u.Carriages).ThenInclude(u => u.CarriageType);
            if (tracks == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = "Không tìm thấy chuyến đi nào";
                return NotFound(_response);
            }
            foreach (var track in tracks)
            {
                track.Train.TotalCarriages = track.Train.Carriages.Count();
                track.Train.TotalSeats = track.Train.Carriages.Sum(c => c.Seats.Count);
                track.Train.TotalReservedSeats = track.Train.Carriages.SelectMany(c => c.Seats).Count(s => s.SeatStatus == 0);
                track.Train.TotalFreeSeats = track.Train.TotalSeats - track.Train.TotalReservedSeats;
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = tracks;
            return Ok(tracks);
        }
        [HttpGet("bytrackid/{trackId}")]
        public async Task<ActionResult<ApiResponse>> GetTrackById(int trackId)
        {
            //int miliseconds = 2000;
            //Thread.Sleep(miliseconds);
            Track foundTrack = _db.Tracks
                .Include(u => u.Train).ThenInclude(u => u.Carriages).ThenInclude(u => u.Seats)
                .Include(u => u.Train).ThenInclude(u => u.Carriages).ThenInclude(u => u.CarriageType).FirstOrDefault(u => u.Id == trackId);
            if (foundTrack == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = "Chuyến đi không tồn tại, vui lòng kiểm tra lại";
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = foundTrack;
            return Ok(foundTrack);
        }

        [HttpGet("byseatid/{seatId}")]
        public async Task<ActionResult<ApiResponse>> GetTrackBySeatId(int seatId)
        {
            Track foundTrack = _db.Tracks.Include(u => u.Train).ThenInclude(u => u.Carriages).ThenInclude(u => u.Seats).FirstOrDefault(u => u.Train.Carriages.Any(s => s.Seats.Any(s => s.Id == seatId)));
            if(foundTrack == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = "Không tìm được ghế";
                return NotFound(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = foundTrack;
            return Ok(foundTrack);
        }

        [HttpGet("find")]
        // Tìm ra chuyến tàu dựa theo các tham số được truyền vào như sau:
        // ga đi, ga đến, thời gian khởi hành
        public async Task<ActionResult<ApiResponse>> FindTrack(string departureStation, DateTime departureTime, string arrivalStation)
        {
            // Bắt đầu tìm thôi
            int miliseconds = 2000;
            Thread.Sleep(miliseconds);
            var foundTrackFromDb = _db.Tracks
                .Include(u => u.Train).ThenInclude(u => u.Carriages).ThenInclude(u => u.Seats)
                .Include(u => u.Train).ThenInclude(u => u.Carriages).ThenInclude(u => u.CarriageType)
                .Where(u => u.DepartureStation == departureStation)
                // 00:00:00 của ngày đó và 23:59:59 của ngày đó
                .Where(u => u.DepartureTime.Date == departureTime.Date && u.DepartureTime.TimeOfDay >= departureTime.TimeOfDay)
                .Where(u => u.ArrivalStation == arrivalStation);
            if (foundTrackFromDb == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = "Không tìm thấy chuyến đi nào phù hợp với yêu cầu của bạn";
                return NotFound(_response);
            }
            foreach(var track in foundTrackFromDb)
            {
            track.Train.TotalCarriages = track.Train.Carriages.Count();
            track.Train.TotalSeats = track.Train.Carriages.Sum(c => c.Seats.Count);
            track.Train.TotalReservedSeats = track.Train.Carriages.SelectMany(c => c.Seats).Count(s => s.SeatStatus == 0);
            track.Train.TotalFreeSeats = track.Train.TotalSeats - track.Train.TotalReservedSeats;
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = foundTrackFromDb;
            return Ok(foundTrackFromDb);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateNewTrainTracks([FromBody] TrackCreateRequestDTO dto)
        {
            Track existedTrainTracks = _db.Tracks
                .Include(u => u.Train)
                .FirstOrDefault(u => u.DepartureStation == dto.DepartureStation && u.DepartureTime == dto.DepartureTime && u.ArrivalStation == dto.ArrivalStation);
            if (existedTrainTracks != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = "Chuyến đi bạn muốn tạo đã tồn tại";
                return BadRequest(_response);
            }

            Track newTrack = new()
            {
                DepartureStation = dto.DepartureStation,
                ArrivalStation = dto.ArrivalStation,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                TrainId = dto.TrainId,
                Train = null
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
