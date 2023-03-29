using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
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

        [HttpGet("get-cart")]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
        {
            try
            {
                ShoppingCart shoppingCart;
                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new();
                }
                else
                {
                    shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems)
                    .ThenInclude(u => u.Seat)
                    .FirstOrDefault(u => u.UserId == userId);
                }
                // Trong cart thì chỉ có cart items, còn trong cart items thì mới có menu items nên phải dùng ThenInclude
                if (shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
                {
                    shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => 1 * u.Seat.SeatPrice);
                }
                _response.Data = shoppingCart;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.ToString();
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        [HttpPost("add-to-cart")]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, [FromBody] CartRequestDTO model)
        {
            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
            Seat seat = _db.Seats.FirstOrDefault(u => u.Id == model.SeatId);
            if (seat == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages = "Lỗi!";
                return NotFound(_response);
            }
            if (shoppingCart == null)
            {
                // Trường hợp 1: Người dùng chưa có giỏ hàng

                // Create a shopping cart for current user in the database & add cart item if the user hasn't add any items to the cart
                ShoppingCart newCart = new() { UserId = userId };
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
                }
                return _response;
            }
        }
    }
}
