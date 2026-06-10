using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels.Allergen;

namespace WebApp.ViewModels.Menu
{
    public class MenuDetailsVM
    {
        public int Id { get; set; }
        [Display(Name = "Food name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Food category")]

        public string CategoryName { get; set; }

        [Display(Name = "Food weight")]

        public decimal? Weight { get; set; }

        [Display(Name = "Food price")]

        public decimal Price { get; set; }

        [Display(Name = "Food description")]

        public string? Description { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }

        public List<AllergenVM> Allergens { get; set; }
    }
}
