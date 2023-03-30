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
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly ApplicationDbContext _db;
        public ShoppingCartController(ApplicationDbContext db)
        {
            _response = new();
            _db = db;
        }
        // 26a3cb2a-360a-451d-b69f-da1c93c0f9fe
        // e8507a51-c99a-4077-9663-20b94db08f7b Khoi
        [HttpGet("get-cart")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(string userId)
        {   
                ShoppingCart shoppingCart;
                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new();
                }
                else
                {
                    shoppingCart =  _db.ShoppingCarts.Include(u => u.CartItems)
                    .ThenInclude(u => u.Seat).ThenInclude(u => u.Carriage).ThenInclude(u => u.Train).ThenInclude(u => u.Track)
                    .Include(u => u.CartItems)
                    .ThenInclude(u => u.Seat).ThenInclude(u => u.Carriage).ThenInclude(u => u.CarriageType)
                    .FirstOrDefault(u => u.UserId == userId);
                }
            if (shoppingCart != null && shoppingCart.CartItems.Count > 0)
            {
                shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => shoppingCart.CartItems.Count * u.Seat.SeatPrice);
            }
            return Ok(shoppingCart);
            
        }

        [HttpPost("add-to-cart")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart([FromBody] CartRequestDTO model)
        {
            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == model.UserId);
            Seat seat = _db.Seats.FirstOrDefault(u => u.Id == model.SeatId);
            if (seat == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages = "Lỗi! Không tìm được chiếc ghế bạn yêu cầu";
                return NotFound(_response);
            }
            if(seat.SeatStatus == SD.status_reserved)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = "Ghế đã được đặt, vui lòng chọn ghế khác";
                return BadRequest(_response);
            }
            if (shoppingCart == null)
            {
                // Trường hợp 1: Người dùng chưa có giỏ hàng

                // Tạo giỏ hàng mới trong database và thêm sản phẩm người dùng vừa mới gửi từ request vào
                ShoppingCart newCart = new() { UserId = model.UserId };
                _db.ShoppingCarts.Add(newCart);
                _db.SaveChanges();
                // Thêm sản phẩm đó vào giỏ dựa theo RequestDTO
                CartItem newCartItem = new()
                {
                    SeatId = model.SeatId,
                    ShoppingCartId = newCart.Id,
                    // If you don't set this to null, it will create a new MenuItem in the MenuItem table
                    Seat = null
                };
                _db.CartItems.Add(newCartItem);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Data = shoppingCart;
                return Ok(_response);
            }
            else
            {    
                CartItem cartItemInCart = shoppingCart.CartItems.FirstOrDefault(u => u.SeatId == model.SeatId);
                // Nếu sản phẩm đó hiện tại đang không có trong giỏ hàng
                if (cartItemInCart == null)
                {
                    // Thì ta sẽ tạo một cái CartItem mới trong giỏ hàng đó, với các thông tin người dùng truyền vào
                    CartItem newCartItem = new()
                    {
                        SeatId = model.SeatId,
                        ShoppingCartId = shoppingCart.Id,                        
                        // Nếu không set cái này là null thì sẽ lỗi
                        Seat = null
                    };
                    _db.CartItems.Add(newCartItem);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Data = shoppingCart;
                    return Ok(_response);
                } else
                {
                    if (shoppingCart.CartItems.Count() == 0)
                    {
                        _db.ShoppingCarts.Remove(shoppingCart);
                    }
                    _db.CartItems.Remove(cartItemInCart);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.Accepted;
                    return Accepted(_response);
                }
            }
        }
        [HttpDelete("remove-from-cart")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> RemoveFromCart(int cartItemId, string userId)
        {
            CartItem cartItemToBeRemoved = _db.CartItems.FirstOrDefault(u => u.Id == cartItemId);
            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
            if (cartItemToBeRemoved == null) {
                return BadRequest();
            }
            _db.Remove(cartItemToBeRemoved);
            _db.SaveChanges();
            if(shoppingCart.CartItems.Count() <= 0)
            {
                _db.Remove(shoppingCart);
                _db.SaveChanges();
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Accepted;
            return Accepted(_response);
        }
    }
}
