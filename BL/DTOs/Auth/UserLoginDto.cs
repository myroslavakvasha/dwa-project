using System.ComponentModel.DataAnnotations;

namespace BL.DTOs.Auth
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
