using DAL.Models;
using Microsoft.EntityFrameworkCore;
using DAL.DTOs.Allergen;

namespace DAL.Services
{
    public class AllergenService
    {
        private readonly GrillPizzaOrdersContext _context;
        private readonly LogService _logService;

        public AllergenService(GrillPizzaOrdersContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public List<AllergenResponseDto> GetAll()
            => _context.Allergens.Select(x => new AllergenResponseDto { Id = x.Id, Name = x.Name }).ToList();

        public AllergenResponseDto GetById(int id)
        {
            Allergen? allergen = _context.Allergens.FirstOrDefault(x => x.Id == id);
            if (allergen == null)
            {
                _logService.LogAction("ERROR", $"Cannot find allergen with id={id}");
                throw new Exception("No allergen with such Id exists");
            }
                
            return new AllergenResponseDto { Id = allergen.Id, Name = allergen.Name };
        }

        public AllergenResponseDto Create(AllergenRequestDto createdAllergen)
        {
            if (_context.Allergens.Any(x => x.Name == createdAllergen.Name))
            {
                _logService.LogAction("ERROR", $"Attempt to create already existing allergen ({createdAllergen.Name}).");
                throw new Exception("Allergen already exists");
            }

            var allergen = new Allergen { Name = createdAllergen.Name };
            _context.Add(allergen);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Allergen with id={allergen.Id} has been created.");

            return new AllergenResponseDto { Id = allergen.Id, Name = allergen.Name };
        }

        public AllergenResponseDto Update(int id, AllergenRequestDto updatedCAllergen)
        {
            Allergen? allergen = _context.Allergens.FirstOrDefault(x => x.Id == id);
            if (allergen == null)
            {
                _logService.LogAction("ERROR", $"Cannot update allergen with id={id} (doesn't exist).");
                throw new Exception("No allergen with such Id exists");
            }

            allergen.Id = id;
            allergen.Name = updatedCAllergen.Name;

            _context.SaveChanges();

            _logService.LogAction("INFO", $"Allergen with id={allergen.Id} has been updated.");

            return new AllergenResponseDto { Id = allergen.Id, Name = allergen.Name };
        }

        public void Delete(int id)
        {
            Allergen? allergen = _context.Allergens.FirstOrDefault(x => x.Id == id);
            if (allergen == null)
            {
                _logService.LogAction("ERROR", $"Attempt to delete non-existing allergen with id={id}.");
                throw new Exception("No allergen with such Id exists");
            }

            //if (allergen.Foods.Any())
            //{
            //    _logService.LogAction("ERROR", $"Cannot delete allergen with id={id} (FK constraint).");
            //    throw new InvalidOperationException("Allergen is assigned to a food");
            //}

            _context.Remove(allergen);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Allergen with id={allergen.Id} has been deleted.");
        }
    }
}
