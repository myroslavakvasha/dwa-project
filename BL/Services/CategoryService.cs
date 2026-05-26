using BL.Models;
using System.Data;
using BL.DTOs.Category;

namespace BL.Services
{
    public class CategoryService
    {
        private readonly GrillPizzaOrdersContext _context;
        private readonly LogService _logService;

        public CategoryService(GrillPizzaOrdersContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public List<CategoryResponseDto> GetAll() 
            => _context.Categories.Select(x => new CategoryResponseDto {Id = x.Id, Name = x.Name }).ToList();

        public CategoryResponseDto GetById(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                _logService.LogAction("ERROR", $"Cannot find category with id={id}");
                throw new Exception("No category with such Id exists");
            }

            return new CategoryResponseDto { Id = category.Id, Name = category.Name };
        }

        public CategoryResponseDto Create(CategoryRequestDto createdCategory)
        {
            if (_context.Categories.Any(x => x.Name == createdCategory.Name))
            {
                _logService.LogAction("ERROR", $"Attempt to create already existing category ({createdCategory.Name}).");
                throw new Exception("Category already exists");
            }

            var category = new Category { Name = createdCategory.Name };
            _context.Add(category);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Category with id={category.Id} has been created.");

            return new CategoryResponseDto { Id = category.Id, Name= category.Name };
        }

        public CategoryResponseDto Update(int id, CategoryRequestDto updatedCategory)
        {
            Category? category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                _logService.LogAction("ERROR", $"Cannot update category with id={id} (doesn't exist).");
                throw new Exception("No category with such Id exists");
            }

            if (_context.Categories.Any(x => x.Name == updatedCategory.Name && x.Id != id))
            {
                _logService.LogAction("ERROR", $"Attempt to update category {updatedCategory.Name} (same category already exists).");
                throw new Exception("Category already exists");
            }

            category.Id = id;
            category.Name = updatedCategory.Name;

            _context.SaveChanges();

            _logService.LogAction("INFO", $"Category with id={category.Id} has been updated.");

            return new CategoryResponseDto { Id = category.Id, Name = category.Name };
        }

        public void Delete(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                _logService.LogAction("ERROR", $"Attempt to delete non-existing category with id={id}.");
                throw new Exception("No category with such Id exists");
            }

            if (_context.Foods.Any(x => x.CategoryId == id))
            {
                _logService.LogAction("ERROR", $"Cannot delete category with id={id} (referenced in existing food).");
                throw new InvalidOperationException("Category has foods assigned to it");
            }

            _context.Remove(category);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Category with id={category.Id} has been deleted.");
        }
    }
}
