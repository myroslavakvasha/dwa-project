using AutoMapper;
using BL.DTOs.Allergen;
using BL.DTOs.Category;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Allergen;
using WebApp.ViewModels.Category;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AllergenController : Controller
    {
        private readonly AllergenService _service;
        private readonly IMapper _mapper;

        public AllergenController(AllergenService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: AllergenController
        public ActionResult Index()
        {
            try
            {
                var allergenResponseDtos = _service.GetAll();
                var allergenVM = allergenResponseDtos.Select(x => _mapper.Map<AllergenVM>(x)).ToList();
                return View(allergenVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: AllergenController/Create
        public ActionResult Create() => View(new AllergenVM());

        // POST: AllergenController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AllergenVM allergenVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(allergenVM);

                var allergenRequestDto = _mapper.Map<AllergenRequestDto>(allergenVM);
                _service.Create(allergenRequestDto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: AllergenController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var allergenVM = _mapper.Map<AllergenVM>(_service.GetById(id));
                return View(allergenVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // POST: AllergenController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AllergenVM allergenVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(allergenVM);

                var allergenRequestDto = _mapper.Map<AllergenRequestDto>(allergenVM);
                _service.Update(id, allergenRequestDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: AllergenController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var allergenVM = _mapper.Map<AllergenVM>(_service.GetById(id));
                return View(allergenVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // POST: AllergenController/Delete/5
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
