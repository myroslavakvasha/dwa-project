using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.Auth
{
    public class UserChangePasswordDto
    {

        [Required(ErrorMessage = "Password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string NewPassword { get; set; }
    }
}
