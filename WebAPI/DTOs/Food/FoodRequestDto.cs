namespace WebAPI.DTOs.Food
{
    public class FoodRequestDto
    {
        public string Name { get; set; } = null!;

        public int CategoryId { get; set; }

        public decimal? Weight { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }
    }
}
