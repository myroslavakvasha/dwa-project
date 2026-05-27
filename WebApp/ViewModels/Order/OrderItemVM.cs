using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Order
{
    public class OrderItemVM
    {
        [Display(Name = "Food name")]
        public string? FoodName { get; set; }

        [Display(Name = "Total price")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
}
