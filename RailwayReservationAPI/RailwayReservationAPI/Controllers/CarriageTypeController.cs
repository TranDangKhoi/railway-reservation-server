using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using System.Net;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/carriage-type")]
    [ApiController]
    public class CarriageTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public CarriageTypeController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCarriageType([FromBody] CarriageTypeCreateRequestDTO dto)
        {
            CarriageType existedCarriageType = _db.CarriageTypes.FirstOrDefault(u => u.Name == dto.CarriageTypeName);
            if (existedCarriageType != null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = "Đã tồn tại kiểu toa tàu";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            CarriageType newCarriageType = new()
            {
                Name = dto.CarriageTypeName,
            };
            _db.Add(newCarriageType);
            _db.SaveChanges();
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            _response.Data = newCarriageType;
            return Ok(_response);
        }
    }
}
