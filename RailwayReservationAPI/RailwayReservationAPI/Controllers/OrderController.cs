using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using RailwayReservationAPI.Utility;
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
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetOrders()
        {
            try
            {
                var orderHeaders = _db.OrderHeaders
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Seat)
                    .OrderByDescending(u => u.OrderHeaderId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = orderHeaders;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.ToString();
            }
            return _response;
        }

        
        [HttpGet("by-user-id/{userId}")]
        public async Task<ActionResult<ApiResponse>> GetOrderByUserId(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var orderHeaders = _db.OrderHeaders
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Seat)
                    .ThenInclude(u => u.Carriage)
                    .ThenInclude(u => u.Train)
                    .ThenInclude(u => u.Track)
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Seat)
                    .ThenInclude(u => u.Carriage)
                    .ThenInclude(u => u.CarriageType)
                    .Include(u => u.ApplicationUser)
                    .Where(u => u.ApplicationUser.Id == userId)
                    .OrderByDescending(u => u.OrderDate);
                if (orderHeaders == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Data = orderHeaders;
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

        [HttpGet("by-order-id/{orderId}")]
        public async Task<ActionResult<ApiResponse>> GetOrderDetailByOrderId(int orderId)
        {
            try
            {
                OrderHeader orderHeaderFoundFromDb = _db.OrderHeaders
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Seat)
                    .ThenInclude(u => u.Carriage)
                    .ThenInclude(u => u.Train)
                    .ThenInclude(u => u.Track)
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Seat)
                    .ThenInclude(u => u.Carriage)
                    .ThenInclude(u => u.CarriageType)
                    .Include(u => u.ApplicationUser)
                    .FirstOrDefault(u => u.OrderHeaderId == orderId);
                if (orderHeaderFoundFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Data = orderHeaderFoundFromDb;
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDTO)
        {
            // Cái đoạn này á, thực ra là truyền vào mấy cái thông tin (PickupEmail, PickupPhoneNumber, ...) này cũng được, hợp lí khi để cho 
            // user không cần đăng nhập mà vẫn có thể order các sản phẩm như bình thường và chỉ cần truyền thông tin của họ vô form, 
            // nếu đăng nhập rồi thì auto-fill form cho họ thôi là cũng chuẩn chỉ, nếu không có mấy cái này trong OrderHeader thì sẽ khá
            // bất hợp lí
            try
            {
                OrderHeader order = new()
                {
                    ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                    PickupEmail = orderHeaderDTO.PickupEmail,
                    PickupName = orderHeaderDTO.PickupName,
                    PickupPhoneNumber = orderHeaderDTO.PickupPhoneNumber,
                    OrderTotal = orderHeaderDTO.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentIntentID = orderHeaderDTO.StripePaymentIntentID,
                    TotalItems = orderHeaderDTO.TotalItems,
                    Status = String.IsNullOrEmpty(orderHeaderDTO.Status) ? SD.order_pending : orderHeaderDTO.Status,
                };

                if (ModelState.IsValid)
                {
                    _db.OrderHeaders.Add(order);
                    _db.SaveChanges();
                    foreach (var orderDetailDTO in orderHeaderDTO.OrderDetailsDTO)
                    {
                        OrderDetail orderDetail = new()
                        {
                            OrderHeaderId = order.OrderHeaderId,
                            SeatId = orderDetailDTO.SeatId,
                            Price = orderDetailDTO.Price,
                        };
                        _db.OrderDetails.Add(orderDetail);
                    }
                    _db.SaveChanges();
                    _response.Data = order;
                    order.OrderDetails = null;
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.ToString();
            }
            return _response;
        }

        [HttpPut("update-status")]
        public async Task<ActionResult<ApiResponse>> UpdateOrderHeader([FromBody] OrderHeaderUpdateDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                OrderHeader orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderId == dto.OrderHeaderId);
                if (orderFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (!string.IsNullOrEmpty(dto.Status))
                {
                    orderFromDb.Status = dto.Status;
                }
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
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
