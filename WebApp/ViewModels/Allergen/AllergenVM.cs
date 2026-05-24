using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Allergen
{
    public class AllergenVM
    {
        public int Id { get; set; }

        [Display(Name = "Allergen name")]
        [Required(ErrorMessage = "Allergen is without a name T_T")]
        public string Name { get; set; } = null!;
    }
}
