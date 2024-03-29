﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet("{carriageId}")]
        public async Task<ActionResult<ApiResponse>> GetCarriageDetailsById(int carriageId)
        {
            Carriage foundCarriageFromDb = _db.Carriages.Include(c => c.Seats).Include(c => c.CarriageType).FirstOrDefault(c => c.Id == carriageId);
            if (foundCarriageFromDb == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = "Không tìm thấy toa tàu";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = foundCarriageFromDb;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCarriage([FromBody]CarriageCreateRequestDTO dto)
        {
            Carriage carriageToBeCreated = new()
            {
                CarriageNo = dto.CarriageNo,
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

        [HttpPut("{carriageId}")]
        public async Task<ActionResult<ApiResponse>> UpdateCarriage(int carriageId, [FromBody] CarriageUpdateRequestDTO dto)
        {
            
            Carriage foundCarriageFromDb = await _db.Carriages.FindAsync(carriageId);
            if (foundCarriageFromDb == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            foundCarriageFromDb.TrainId = dto.TrainId;
            foundCarriageFromDb.CarriageTypeId = dto.CarriageTypeId;
            _db.Update(foundCarriageFromDb);
            _db.SaveChanges();
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = "Cập nhật thành công";
            return Ok(foundCarriageFromDb);

        }
    }
}
