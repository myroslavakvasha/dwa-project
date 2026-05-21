using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Services;
using DAL.DTOs.Food;
using DAL.DTOs.Allergen;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AllergenController : ControllerBase
    {
        private readonly AllergenService _service;

        public AllergenController(AllergenService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<AllergenResponseDto>> GetAll()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AllergenResponseDto> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<AllergenResponseDto> Create([FromBody] AllergenRequestDto createdAllergen)
        {
            try
            {
                AllergenResponseDto allergenResponseDto = _service.Create(createdAllergen);
                var uri = Url.Action(nameof(GetById));

                return Created(uri, allergenResponseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult<AllergenResponseDto> Update([FromRoute] int id, [FromBody] AllergenRequestDto updatedAllergen)
        {
            try
            {
                AllergenResponseDto allergenResponseDto = _service.Update(id, updatedAllergen);
                return Ok(allergenResponseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
