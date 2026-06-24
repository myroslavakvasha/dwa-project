using BL.DTOs.Auth;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Security;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GrillPizzaOrdersContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthService _service;

        public AuthController(IConfiguration configuration, GrillPizzaOrdersContext context, AuthService service)
        {
            _configuration = configuration;
            _context = context;
            _service = service;
        }

        //[HttpGet("[action]")]
        //public ActionResult GetToken()
        //{
        //    try
        //    {
        //        var secureKey = _configuration["JWT:SecureKey"];
        //        var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);

        //        return Ok(serializedToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("genhash/{password}")]
        //public IActionResult GenHash(string password)
        //{
        //    var salt = PasswordHashProvider.GetSalt();
        //    var hash = PasswordHashProvider.GetHash(password, salt);
        //    return Ok(new { salt, hash });
        //}

        [HttpPost("[action]")]
        public ActionResult Register(UserRegisterDto userDto)
        {
            try
            {
                _service.Register(userDto);
                return Ok("Registered successfully");

            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(UserLoginDto userDto)
        {
            try
            {
                UserResponseDto? user = _service.ValidateUser(userDto.Username, userDto.Password);
                if (user == null)
                    return BadRequest("Incorrect username or password");

                // Create and return JWT token
                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 30, user.Username, user.RoleTitle);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("[action]")]
        public ActionResult ChangePassword(UserChangePasswordDto userDto)
        {
            try
            {
                var username = User.Identity?.Name;
                _service.ChangePassword(username!, userDto.OldPassword, userDto.NewPassword);

                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
