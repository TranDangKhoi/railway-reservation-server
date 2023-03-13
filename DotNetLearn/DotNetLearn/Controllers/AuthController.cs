using DotNetLearn.Data;
using DotNetLearn.Models;
using DotNetLearn.Models.Dto;
using DotNetLearn.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DotNetLearn.Controllers
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
            _response = new ApiResponse();
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            // Tìm trong db có thằng E-mail nào trùng với thằng E-mail người dùng nhập vào hay không?
            ApplicationUser userFromDb = _db.ApplicationUsers
                .FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
            // Nếu trùng thì thôi trả về 422 Unprocessable Entity

            if (userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = "E-mail đã tồn tại";
                return BadRequest(_response);
            }

            ApplicationUser newUser = new()
            {
                UserName = model.Email,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Fullname = model.Fullname
            };
            
            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    // Role của  người mới tạo mặc định sẽ là user 
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
                            new Claim(ClaimTypes.Role, SD.Role_Admin)
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
            _response.ErrorMessage = "Có lỗi đã xảy ra! Vui lòng thử lại";
            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO model)
        {
            // Tìm trong database xem có thằng E-Mail nào trùng với E-mail mà người dùng truyền vào hay không
            ApplicationUser userFromDb = _db.ApplicationUsers
                    .FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
            
            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);
            // Nếu không có thì trả ra status 422 Unprocessable Entity
            if (isValid == false) {
                _response.StatusCode = HttpStatusCode.UnprocessableEntity;
                _response.IsSuccess = false;
                _response.ErrorMessage = "E-mail hoặc password không chính xác";
                return NotFound(_response);
            }

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
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessage = "E-mail hoặc mật khẩu không chính xác";
                return NotFound(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Data = loginResponse;
            return Ok(_response);
        }
    }
}
