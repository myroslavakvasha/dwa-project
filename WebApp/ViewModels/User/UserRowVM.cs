using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.User
{
    public class UserRowVM
    {
        [Display(Name = "Username")]
        public string Username { get; set; } = null!;

        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone")]
        public string Phone { get; set; } = null!;
    }
}
