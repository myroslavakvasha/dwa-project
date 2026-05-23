using AutoMapper;
using BL.DTOs.Food;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Security.AccessControl;
using WebApp.Models.Food;

namespace WebApp.Controllers
{
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

        // GET: FoodController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FoodController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FoodController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FoodController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FoodController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FoodController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
