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
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/Login")]
    public class LoginControllerV2 : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userservice;
        private readonly JwtService _jwtService;

        public LoginControllerV2(ILogger<LoginController> logger, IUserService userservice, JwtService jwtService)
        {
            _logger = logger;
            _userservice = userservice;
            _jwtService = jwtService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            var user = await _userservice.GetUserByEmail(logindto.username);
            if (user == null) return Unauthorized("User Not Found");
            //var hashedPassword = BCrypt.Net.BCrypt.HashPassword(logindto.password);
            // verify during login
            //bool isValid = BCrypt.Net.BCrypt.Verify(password, storedHash);
            bool isValid = BCrypt.Net.BCrypt.Verify(logindto.password, user.Password);

            if (!isValid)
                return Unauthorized("Invalid Credentials!");

            var token = _jwtService.GenerateToken(logindto.username);
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,      // 🔒 Cannot access from JS
                Secure = true,        // Only HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            var currentuser = _userservice.PrettifyUser(user);
            return Ok(new { Token = token, User = currentuser });
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetUserDetails()
        {
            var users = await _userservice.GetAllUsers();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDto dto)
        {
            
            if (dto.Otp == null)
            {
                var otp = await _userservice.GenerateOtp(dto);

                return Ok(otp);
            }
            else
            {
                var isadded = await _userservice.CreateUser(dto);
                return Ok(isadded);
            }
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

