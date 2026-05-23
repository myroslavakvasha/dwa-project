using BL.DTOs.Allergen;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FoodAllergenController : ControllerBase
    {
        private readonly FoodAllergenService _service;

        public FoodAllergenController(FoodAllergenService service)
        {
            _service = service;
        }

        // get all allergens fot given food
        [HttpGet("{foodId}")]
        public ActionResult<List<AllergenResponseDto>> GetAll([FromRoute] int foodId)
        {
            try
            {
                return _service.GetAllAllergensForFood(foodId);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AssignAllergen([FromQuery] int foodId, [FromQuery] int allergenId)
        {
            try
            {
                _service.AddAllergenToFood(foodId, allergenId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult RemoveAllergen([FromQuery] int foodId, [FromQuery] int allergenId)
        {
            try
            {
                _service.RemoveAllergenFromFood(foodId, allergenId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
