using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using System.Net;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarriageController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public CarriageController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCarriage(int carriageTypeId, [FromBody]CarriageCreateRequestDTO dto)
        {
            Carriage carriageToBeCreated = new()
            {
                CarriageNo = dto.CarriageNo,
                CarriageTypeId = carriageTypeId,
                Seats = null,
                CarriageType = null,
            };
            _db.Add(carriageToBeCreated);
            _db.SaveChanges();           
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            _response.Data = carriageToBeCreated;
            return Ok(carriageToBeCreated);
        }
    }
}
