using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs.Allergen;

namespace WebAPI.Services
{
    public class FoodAllergenService
    {
        private readonly GrillPizzaOrdersContext _context;
        private readonly LogService _logService;

        public FoodAllergenService(GrillPizzaOrdersContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public List<AllergenResponseDto> GetAllAllergensForFood(int foodId)
        {
            Food? food = _context.Foods.Include(x => x.Allergens).FirstOrDefault(x => x.Id == foodId);
            if(food == null)
            {
                _logService.LogAction("ERROR", $"Cannot find food with id={foodId}");
                throw new Exception("No food with such Id exists");
            }

            return food.Allergens
                .Select(x => new AllergenResponseDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
        }

        public void AddAllergenToFood(int foodId, int allergenId)
        {
            Food? food = _context.Foods.Include(x => x.Allergens).FirstOrDefault(x => x.Id == foodId);
            if (food == null)
            {
                _logService.LogAction("ERROR", $"Cannot find food with id={foodId}");
                throw new Exception("No food with such Id exists");
            }

            Allergen? allergen = _context.Allergens.FirstOrDefault(x => x.Id == allergenId);
            if (allergen == null)
            {
                _logService.LogAction("ERROR", $"Attempt to assign non-existing allergen with id={allergenId}.");
                throw new Exception("No allergen with such Id exists");
            }

            if (food.Allergens.Any(x => x.Id == allergenId))
            {
                _logService.LogAction("ERROR", $"Cannot assign allergen with id={allergenId} (already assigned).");
                throw new InvalidOperationException("Allergen already assigned to this food");
            }

            food.Allergens.Add(allergen);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Allergen with id={allergenId} has been added to food (id={foodId})");
        }

        public void RemoveAllergenFromFood(int foodId, int allergenId)
        {
            Food? food = _context.Foods.Include(x => x.Allergens).FirstOrDefault(x => x.Id == foodId);
            if (food == null)
            {
                _logService.LogAction("ERROR", $"Cannot find food with id={foodId}");
                throw new Exception("No food with such Id exists");
            }

            Allergen? allergen = _context.Allergens.FirstOrDefault(x => x.Id == allergenId);
            if (allergen == null)
            {
                _logService.LogAction("ERROR", $"Attempt to remove non-existing allergen with id={allergenId}.");
                throw new Exception("No allergen with such Id exists");
            }

            if (!food.Allergens.Any(x => x.Id == allergenId))
            {
                _logService.LogAction("ERROR", $"Cannot remove allergen with id={allergenId} (already removed).");
                throw new InvalidOperationException("Allergen is not assigned to this food");
            }

            food.Allergens.Remove(allergen);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Allergen with id={allergenId} has been removed from food (id={foodId})");
        }
    }
}
