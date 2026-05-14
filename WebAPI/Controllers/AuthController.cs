using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs.Auth;
using WebAPI.Security;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GrillPizzaOrdersContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, GrillPizzaOrdersContext context)
        {
            _configuration = configuration;
            _context = context;
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
        public ActionResult<UserRegisterDto> Register(UserRegisterDto userDto)
        {
            try
            {
                // Check if there is such a username in the database already
                var trimmedUsername = userDto.Username.Trim();
                if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                    return BadRequest($"Username {trimmedUsername} already exists");

                // Get the role from the database
                Role? role = _context.Roles.FirstOrDefault(x => x.RoleTitle.Equals("User"));
                if(role == null) return StatusCode(500);
                var roleId = role.Id;

                // Hash the password
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, b64salt);

                // Create user from DTO and hashed password
                var user = new User
                {
                    Username = userDto.Username,
                    RoleId = roleId,
                    PwdHash = b64hash,
                    PwdSalt = b64salt,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.Phone,
                };

                // Add user and save changes to database
                _context.Add(user);
                _context.SaveChanges();

                return Ok("Registered successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(UserLoginDto userDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                // Try to get a user from database
                var existingUser = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username == userDto.Username);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                // Check is password hash matches
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return BadRequest(genericLoginFail);

                // Create and return JWT token
                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, existingUser.Username, existingUser.Role.RoleTitle);

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
                var genericLoginFail = "Incorrect username or password";

                // Getting username from the token
                var username = User.Identity?.Name;

                // Try to get a user from database
                var existingUser = _context.Users.FirstOrDefault(x => x.Username == username);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                // Check is password hash matches
                var b64hash = PasswordHashProvider.GetHash(userDto.OldPassword, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return BadRequest(genericLoginFail);

                // Changing the password
                var b64saltNew = PasswordHashProvider.GetSalt();
                var b64hashNew = PasswordHashProvider.GetHash(userDto.NewPassword, b64saltNew);

                existingUser.PwdSalt = b64saltNew;
                existingUser.PwdHash = b64hashNew;

                _context.SaveChanges();
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
