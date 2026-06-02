using AutoMapper;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebApp.ViewModels.Food;
using WebApp.ViewModels.Menu;

namespace WebApp.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly FoodService _foodService;
        private readonly CategoryService _categoryService;
        private IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MenuController(FoodService foodService, IConfiguration configuration, IMapper mapper, CategoryService categoryService)
        {
            _foodService = foodService;
            _configuration = configuration;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        // GET: MenuController
        public ActionResult Index(MenuIndexVM menuVM)
        {
            try
            {
                var size = 10;
                var foodFiltered = _foodService.Search(menuVM.Q, menuVM.Q, menuVM.CategoryId == 0 ? null : menuVM.CategoryId, menuVM.Page, size);
                var filteredCount = _foodService.GetFilteredCount(menuVM.Q, menuVM.Q, menuVM.CategoryId == 0 ? null : menuVM.CategoryId);

                menuVM.LastPage = (int)Math.Ceiling(1.0 * filteredCount / size);

                menuVM.Foods = foodFiltered.Select(x => _mapper.Map<FoodRowVM>(x)).ToList();

                var categories = _categoryService.GetAll();
                menuVM.Categories = new SelectList(categories, "Id", "Name");

                return View(menuVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: MenuController/Details/5
        public ActionResult Details(int id)
        {
            var food = _mapper.Map<FoodRowVM>(_foodService.GetById(id));
            return View(food);
        }
    }
}
