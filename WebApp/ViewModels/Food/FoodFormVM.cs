using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Food
{
    public class FoodFormVM
    {
        public int Id { get; set; }

        [Display(Name = "Food Name")]
        [Required(ErrorMessage = "Food without a name no can do")]
        public string Name { get; set; } = null!;

        [Display(Name = "Food Category")]

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public SelectList Categories { get; set; }

        [Display(Name = "Food Weight")]
        [Range(1, 10000, ErrorMessage = "Not realistic weight")]
        public decimal? Weight { get; set; }

        [Display(Name = "Food Price")]
        [Range(1, 1000, ErrorMessage = "Not realistic price")]
        public decimal Price { get; set; }

        [Display(Name = "Food Description")]

        public string? Description { get; set; }
    }
}
