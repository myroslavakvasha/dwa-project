using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs.Order
{
    public class OrderItemRequestDto
    {
        public int FoodId { get; set; }

        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
