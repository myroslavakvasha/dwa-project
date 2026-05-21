namespace DAL.DTOs.Order
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }

        public string? FoodName { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }
    }
}
