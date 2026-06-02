using BL.DTOs.Allergen;

namespace BL.DTOs.Food
{
    public class FoodResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public decimal? Weight { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<AllergenResponseDto> Allergens { get; set; } = new List<AllergenResponseDto>();
    }
}
