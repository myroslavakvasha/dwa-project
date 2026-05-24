using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Food
{
    public class FoodRowVM
    {
        public int Id { get; set; }
        [Display(Name="Food name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Food category")]

        public string CategoryName { get; set; }

        [Display(Name = "Food weight")]

        public decimal? Weight { get; set; }
        
        [Display(Name = "Food price")]

        public decimal Price { get; set; }

        [Display(Name = "Food description")]

        public string? Description { get; set; }

    }
}
