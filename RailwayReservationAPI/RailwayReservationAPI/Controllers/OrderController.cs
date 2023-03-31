using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using System.Net;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string userId)
        {
            try
            {
                var orderHeaders = _db.OrderHeaders
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Seat)
                    .OrderByDescending(u => u.OrderHeaderId);
                if (!string.IsNullOrEmpty(userId))
                {
                    // Nếu truyền vào userId thì show ra orders của thằng đó thoai
                    _response.Data = orderHeaders.Where(u => u.ApplicationUserId == userId);
                }
                else
                {
                    // Còn nếu không truyền userId nào vô thì show ra tất cả orders lunnn
                    // Cái này để hiển thị trong trang admin
                    _response.Data = orderHeaders;
                }
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.ToString();
            }
            return _response;
        }
    }
}
