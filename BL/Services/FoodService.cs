using Azure;
using BL.DTOs.Allergen;
using BL.DTOs.Food;
using BL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;

namespace BL.Services
{
    public class FoodService
    {
        private readonly GrillPizzaOrdersContext _context;
        private readonly LogService _logService;

        public FoodService(GrillPizzaOrdersContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public List<FoodResponseDto> GetAll() 
            => _context.Foods
            .Include(x => x.Allergens)
            .Select(x => new FoodResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                Weight = x.Weight,
                Price = x.Price,
                Description = x.Description,
                Allergens = x.Allergens
                    .Select(y => new AllergenResponseDto
                    {
                        Id = y.Id,
                        Name = y.Name
                    })
                    .ToList()
            })
            .ToList();

        public FoodResponseDto GetById(int id)
        {
            Food? food = _context.Foods.Include(x => x.Allergens).Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            if (food == null)
            {
                _logService.LogAction("ERROR", $"Cannot find food with id={id}");
                throw new Exception("No food with such Id exists");
            }

            return new FoodResponseDto
            {
                Id = food.Id,
                Name = food.Name,
                CategoryId = food.CategoryId,
                CategoryName = food.Category.Name,
                Weight = food.Weight,
                Price = food.Price,
                Description = food.Description,
                Allergens = food.Allergens
                    .Select(x => new AllergenResponseDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList()
            };
        }

        public FoodResponseDto Create(FoodRequestDto createdFood)
        {
            if (_context.Foods.Any(
                x => x.Name == createdFood.Name
                && x.Weight == createdFood.Weight
                && x.Price == createdFood.Price
                && x.CategoryId==createdFood.CategoryId))
            {
                _logService.LogAction("ERROR", $"Attempt to create already existing food ({createdFood.Name}).");
                throw new Exception("Food already exists");
            }

            if (!_context.Categories.Any(x => x.Id == createdFood.CategoryId))
            {
                _logService.LogAction("ERROR", $"Cannot create food with category id={createdFood.CategoryId} (doesn't exist).");
                throw new Exception("No category with such Id exists");
            }

            var food = new Food
            {
                Name = createdFood.Name,
                CategoryId = createdFood.CategoryId,
                Weight = createdFood.Weight,
                Price = createdFood.Price,
                Description = createdFood.Description
            };

            _context.Add(food);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Food with id={food.Id} has been created.");

            return new FoodResponseDto
            {
                Id = food.Id,
                Name = food.Name,
                CategoryId = food.CategoryId,
                Weight = food.Weight,
                Price = food.Price,
                Description = food.Description
            };
        }

        public FoodResponseDto Update(int id, FoodRequestDto updatedFood)
        {
            Food? food = _context.Foods.FirstOrDefault(x => x.Id == id);
            if (food == null)
            {
                _logService.LogAction("ERROR", $"Cannot update food with id={id} (doesn't exist).");
                throw new Exception("No food with such Id exists");
            }

            if (!_context.Categories.Any(x => x.Id == updatedFood.CategoryId))
            {
                _logService.LogAction("ERROR", $"Cannot update food with category id={updatedFood.CategoryId} (doesn't exist).");
                throw new Exception("No category with such Id exists");
            }

            if (_context.Foods.Any(
                x => x.Name == updatedFood.Name
                && x.Weight == updatedFood.Weight
                && x.Price == updatedFood.Price
                && x.CategoryId == updatedFood.CategoryId
                && x.Id != id))
            {
                _logService.LogAction("ERROR", $"Attempt to update food {updatedFood.Name} (same food already exists).");
                throw new Exception("Food already exists");
            }

            food.Id = id;
            food.Name = updatedFood.Name;
            food.CategoryId = updatedFood.CategoryId;
            food.Weight = updatedFood.Weight;
            food.Price = updatedFood.Price;
            food.Description = updatedFood.Description;

            _context.SaveChanges();

            _logService.LogAction("INFO", $"Food with id={food.Id} has been updated.");

            return new FoodResponseDto
            {
                Id = food.Id,
                Name = food.Name,
                CategoryId = food.CategoryId,
                Weight = food.Weight,
                Price = food.Price,
                Description = food.Description
            };
        }

        public void Delete(int id)
        {
            Food? food = _context.Foods.FirstOrDefault(x => x.Id == id);
            if (food == null)
            {
                _logService.LogAction("ERROR", $"Cannot delete food with id={id} (doesn't exist).");
                throw new Exception("No food with such Id exists");
            }

            if(_context.OrderItems.Any(x => x.FoodId == id))
            {
                _logService.LogAction("ERROR", $"Cannot delete food with id={id} (referenced in existing orders).");
                throw new InvalidOperationException("Food is assigned to an order");
            }

            _context.Remove(food);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Food with id={food.Id} has been deleted.");
        }

        public List<FoodResponseDto> Search(string? name, string? description, int? categoryId, int page, int count)
        {
            if (page < 1) page = 1;
            if (count < 1) count = 10;

            IQueryable<Food> foods = _context.Foods;

            //if (!string.IsNullOrEmpty(name))
            //    foods = foods.Where(x => x.Name.Contains(name));

            //if (!string.IsNullOrEmpty(description))
            //    foods = foods.Where(x => x.Description.Contains(description));

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(description))
                foods = foods.Where(x =>
                    (string.IsNullOrEmpty(name) || x.Name.Contains(name)) ||
                    (string.IsNullOrEmpty(description) || x.Description.Contains(description)));

            if (categoryId.HasValue)
            {
                foods = foods.Where(x => x.CategoryId == categoryId);
            }

            return
                foods
                .Include(x => x.Allergens)
                .Select(x => new FoodResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Weight = x.Weight,
                    Price = x.Price,
                    Description = x.Description,
                    Allergens = x.Allergens.Select(y => new AllergenResponseDto
                    {
                        Id = y.Id,
                        Name = y.Name
                    })
                    .ToList()
                })
                .Skip((page - 1) * count)
                .Take(count)
                .ToList();
        }

        public int GetFilteredCount(string? name, string? description, int? categoryId)
        {
            IQueryable<Food> foods = _context.Foods;

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(description))
                foods = foods.Where(x =>
                    (string.IsNullOrEmpty(name) || x.Name.Contains(name)) ||
                    (string.IsNullOrEmpty(description) || x.Description.Contains(description)));

            if (categoryId.HasValue)
            {
                foods = foods.Where(x => x.CategoryId == categoryId);
            }

            return foods.Count();
        }
    }
}
