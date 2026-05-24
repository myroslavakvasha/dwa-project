using AutoMapper;
using BL.DTOs.Category;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Category;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _service;
        private readonly IMapper _mapper;

        public CategoryController(CategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: CategoryController
        public ActionResult Index()
        {
            try
            {
                var categoryResponseDtos = _service.GetAll();
                var categoryVM = categoryResponseDtos.Select(x => _mapper.Map<CategoryVM>(x));
                return View(categoryVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: CategoryController/Create
        public ActionResult Create() => View(new CategoryVM());

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryVM categoryVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(categoryVM);

                var categoryRequestDto = _mapper.Map<CategoryRequestDto>(categoryVM);
                _service.Create(categoryRequestDto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var categoryVM = _mapper.Map<CategoryVM>(_service.GetById(id));
                return View(categoryVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryVM categoryVM)
        {
            try
            {
                if(!ModelState.IsValid)
                    return View(categoryVM);

                var categoryRequestDto = _mapper.Map<CategoryRequestDto>(categoryVM);
                _service.Update(id, categoryRequestDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var categoryVM = _mapper.Map<CategoryVM>(_service.GetById(id));
                return View(categoryVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
