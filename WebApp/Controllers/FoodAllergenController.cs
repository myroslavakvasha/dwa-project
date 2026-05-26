using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;
using WebApp.ViewModels.Allergen;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodAllergenController : Controller
    {
        private readonly FoodAllergenService _foodAllergenService;
        private readonly AllergenService _allergenService;
        private readonly FoodService _foodService;
        private readonly IMapper _mapper;

        public FoodAllergenController(FoodAllergenService service, IMapper mapper, AllergenService allergenService, FoodService foodService)
        {
            _foodAllergenService = service;
            _mapper = mapper;
            _allergenService = allergenService;
            _foodService = foodService;
        }

        [HttpGet]
        public ActionResult Manage(int foodId)
        {
            try
            {
                var food = _foodService.GetById(foodId);

                var allergenVMs = _foodAllergenService
                    .GetAllAllergensForFood(foodId).Select(x => _mapper.Map<AllergenVM>(x)).ToList();

                FoodAllergenVM foodAllergenVM = new FoodAllergenVM
                {
                    FoodId = foodId,
                    FoodName = food.Name,
                    AssignedAllergens = allergenVMs,
                    AvailableAllergens = new SelectList(
                        _allergenService.GetAll()
                        .Where(x => !allergenVMs.Any(y => y.Id == x.Id))
                        .ToList(), "Id", "Name")
                };

                return View(foodAllergenVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAllergen(int foodId, int SelectedAllergenId)
        {
            try
            {
                if (SelectedAllergenId != 0)
                    _foodAllergenService.AddAllergenToFood(foodId, SelectedAllergenId);

                return RedirectToAction(nameof(Manage), new { foodId = foodId });
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAllergen(int foodId, int allergenId)
        {
            try
            {
                _foodAllergenService.RemoveAllergenFromFood(foodId, allergenId);
                return RedirectToAction(nameof(Manage), new { foodId = foodId });
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

    }
}
