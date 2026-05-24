using AutoMapper;
using BL.DTOs.Food;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Security.AccessControl;
using WebApp.ViewModels.Food;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodController : Controller
    {
        private readonly FoodService _foodService;
        private readonly CategoryService _categoryService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public FoodController(FoodService service, IConfiguration configuration, IMapper mapper, CategoryService categoryService)
        {
            _foodService = service;
            _configuration = configuration;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        // GET: FoodController
        public ActionResult Index(FoodIndexVM foodVM)
        {
            try
            {
                var foodFiltered = _foodService.Search(foodVM.Q, foodVM.Q, foodVM.CategoryId == 0 ? null : foodVM.CategoryId, foodVM.Page, foodVM.Size);

                var filteredCount = _foodService.GetFilteredCount(foodVM.Q, foodVM.Q, foodVM.CategoryId == 0 ? null : foodVM.CategoryId);

                // BEGIN PAGER
                var expandPages = _configuration.GetValue<int>("Paging:ExpandPages");
                foodVM.LastPage = (int)Math.Ceiling(1.0 * filteredCount / foodVM.Size);
                foodVM.FromPager = foodVM.Page > expandPages ?
                  foodVM.Page - expandPages :
                  1;
                foodVM.ToPager = (foodVM.Page + expandPages) < foodVM.LastPage ?
                  foodVM.Page + expandPages :
                  foodVM.LastPage;
                // END PAGER

                foodVM.Foods = foodFiltered.Select(x => _mapper.Map<FoodRowVM>(x)).ToList();

                var categories = _categoryService.GetAll();
                foodVM.Categories = new SelectList(categories, "Id", "Name");

                return View(foodVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: FoodController/Create
        public ActionResult Create()
        {
            try
            {
                var categories = _categoryService.GetAll();
                var foodVM = new FoodFormVM
                {
                    Categories = new SelectList(categories, "Id", "Name")
                };

                return View(foodVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // POST: FoodController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FoodFormVM foodVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var categories = _categoryService.GetAll();
                    foodVM.Categories = new SelectList(categories, "Id", "Name");
                    return View(foodVM);
                }

                var foodRequestDto = _mapper.Map<FoodRequestDto>(foodVM);
                _foodService.Create(foodRequestDto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: FoodController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var categories = _categoryService.GetAll();
                var foodResponseDto = _foodService.GetById(id);

                var foodVM = new FoodFormVM
                {
                    Name = foodResponseDto.Name,
                    CategoryId = foodResponseDto.CategoryId,
                    Categories = new SelectList(categories, "Id", "Name"),
                    Weight = foodResponseDto.Weight,
                    Price = foodResponseDto.Price,
                    Description = foodResponseDto.Description
                };

                return View(foodVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // POST: FoodController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FoodFormVM foodVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var categories = _categoryService.GetAll();
                    foodVM.Categories = new SelectList(categories, "Id", "Name");
                    return View(foodVM);
                }

                var foodRequestDto = _mapper.Map<FoodRequestDto>(foodVM);
                _foodService.Update(id, foodRequestDto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: FoodController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var foodResponseDto = _foodService.GetById(id);

                var foodVm = _mapper.Map<FoodRowVM>(foodResponseDto);
                return View(foodVm);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // POST: FoodController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _foodService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
