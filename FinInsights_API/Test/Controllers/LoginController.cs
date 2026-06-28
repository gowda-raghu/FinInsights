using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Login")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userservice;
        private readonly JwtService _jwtService;

        public LoginController(ILogger<LoginController> logger, IUserService userservice, JwtService jwtService)
        {
            _logger = logger;
            _userservice = userservice;
            _jwtService = jwtService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            var user = await _userservice.GetAllUsers();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            // verify during login
            //bool isValid = BCrypt.Net.BCrypt.Verify(password, storedHash);

            var validUser = user.FirstOrDefault(u =>
                u.Email == username && u.Password == hashedPassword);

            if (validUser == null)
                return Unauthorized();

            var token = _jwtService.GenerateToken(username);
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,      // 🔒 Cannot access from JS
                Secure = true,        // Only HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            return Ok(new { Token = token });
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetUserDetails()
        {
            var users = await _userservice.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email
            };
            var isadded = await _userservice.CreateUser(dto);
            return Ok(isadded);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> AddUser(Guid id)
        {
            var user = await _userservice.GetUserById(id);
            if (user == null)
            {
                return Ok("User Not Found");
            }
            return Ok(user);
        }


    }
}

