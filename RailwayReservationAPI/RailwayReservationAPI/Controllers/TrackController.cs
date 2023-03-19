using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;

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
        public async Task<ActionResult<ApiResponse>> FindTrack(TrackRequestDTO trackRequestDto)
        {
            
        }
    }
}
