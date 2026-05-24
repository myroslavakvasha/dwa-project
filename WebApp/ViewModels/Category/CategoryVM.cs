using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApp.ViewModels.Category
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Display(Name = "Category name")]
        [Required(ErrorMessage = "Category is without a name T_T")]
        public string Name { get; set; } = null!;
    }
}
