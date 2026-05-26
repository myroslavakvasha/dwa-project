using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using WebApp.ViewModels.Allergen;

namespace WebApp.ViewModels
{
    public class FoodAllergenVM
    {
        public int FoodId { get; set; }

        [Display(Name = "Food name")]
        public string FoodName { get; set; }

        public List<AllergenVM> AssignedAllergens { get; set; }

        [ValidateNever]
        public SelectList AvailableAllergens { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select an allergen")]
        public int? SelectedAllergenId { get; set; }
    }
}
