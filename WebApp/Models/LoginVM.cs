using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
