using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RailwayReservationAPI.Data;
using RailwayReservationAPI.Models;
using RailwayReservationAPI.Models.Dto;
using RailwayReservationAPI.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace RailwayReservationAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        public AuthController(ApplicationDbContext db, IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _response = new ApiResponse();
            _userManager = userManager;
            _roleManager = roleManager;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers
                    .FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

            if (isValid == false)
            {
                _response.Data = new LoginResponseDTO();
                _response.StatusCode = HttpStatusCode.UnprocessableEntity;
                _response.IsSuccess = false;
                _response.ErrorMessages = "E-mail hoặc mật khẩu không chính xác";
                return UnprocessableEntity(_response);
            }

            //we have to generate JWT Token
            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("fullName", userFromDb.Fullname),
                    new Claim("id", userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponse = new()
            {
                Access_token = tokenHandler.WriteToken(token),
                ApplicationUser = userFromDb,
            };

            if (loginResponse.Access_token == null || string.IsNullOrEmpty(loginResponse.Access_token))
            {
                _response.StatusCode = HttpStatusCode.UnprocessableEntity;
                _response.IsSuccess = false;
                _response.ErrorMessages = "E-mail hoặc mật khẩu không chính xác";
                return UnprocessableEntity(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Data = loginResponse;
            return Ok(_response);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers
                .FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            if (userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.UnprocessableEntity;
                _response.IsSuccess = false;
                // Replace "Username" with "Email"
                _response.ErrorMessages = "E-mail đã tồn tại, vui lòng sử dụng một e-mail khác";
                return UnprocessableEntity(_response);
            }

            ApplicationUser newUser = new()
            {
                UserName = model.Email.Substring(0, model.Email.IndexOf("@")),
                // Thay thế Username thành Email
                Email = model.Email,
                // Thay thế Username thành Email
                NormalizedEmail = model.Email.ToUpper(),
                Fullname = model.Fullname,
                Avatar = $"https://ui-avatars.com/api/?background=random&name={model.Fullname}"
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    // Thay đổi logic chỗ này, role của  người mới tạo mặc định sẽ là user 
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    // Nếu success thì nên trả ra token luôn
                    // LƯU Ý: PHẢI TẠO RA RegisterResponseDTO.cs để khi api response sẽ trả ra token lun, không cần phải đăng nhập mới trả token
                    JwtSecurityTokenHandler tokenHandler = new();
                    byte[] key = Encoding.ASCII.GetBytes(secretKey);

                    SecurityTokenDescriptor tokenDescriptor = new()
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("fullname", newUser.Fullname),
                            new Claim("id", newUser.Id.ToString()),
                            new Claim(ClaimTypes.Email, newUser.UserName.ToString()),
                            new Claim(ClaimTypes.Role, SD.Role_Customer)
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                    RegisterResponseDTO registerResponse = new()
                    {
                        Access_token = tokenHandler.WriteToken(token),
                        ApplicationUser = newUser,
                    };

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Data = registerResponse;
                    return Ok(_response);
                }
            }
            catch (Exception)
            {

            }
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages = "Có lỗi không ngoài ý muốn!, vui lòng thử lại sau";
            return BadRequest(_response);

        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _response.IsSuccess = true;
            _response.StatusCode=HttpStatusCode.OK;
            _response.Data= null;
            return Ok(_response);
        }


    }
}
