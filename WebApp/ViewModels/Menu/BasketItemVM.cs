using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Menu
{
    public class BasketItemVM
    {
        public int FoodId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }
    }
}
