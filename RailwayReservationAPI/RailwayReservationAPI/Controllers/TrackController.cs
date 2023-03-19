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
        // Tìm ra chuyến tàu dựa theo các tham số được truyền vào như sau:
        // ga đi, ga đến, thời gian khởi hành, thời gian kết thúc
        public async Task<ActionResult<ApiResponse>> FindTrack(string departureStation)
        {
            // Bắt đầu tìm thôi
            var track = _db.Tracks.Include(u => u.TrainTracks)
                .Include(u => u.TrainTracks)
                .ThenInclude(u => u.Train)
                .ThenInclude(u => u.Carriages)
                .Where(u => u.DepartureStation == departureStation);
            if (track == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = track;
            return Ok(track);
        }
    }
}
