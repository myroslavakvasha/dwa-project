using BL.DTOs.Food;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodService _service;

        public FoodController(FoodService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<FoodResponseDto>> GetAll()
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
        public ActionResult<FoodResponseDto> GetById(int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public ActionResult<FoodResponseDto> Create([FromBody] FoodRequestDto createdFood)
        {
            try
            {
                FoodResponseDto foodResponseDto = _service.Create(createdFood);
                var uri = Url.Action(nameof(GetById));

                return Created(uri, foodResponseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult<FoodResponseDto> Update([FromRoute] int id, [FromBody] FoodRequestDto updatedFood)
        {
            try
            {
                FoodResponseDto foodResponseDto = _service.Update(id, updatedFood);
                return Ok(foodResponseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public ActionResult<List<FoodResponseDto>> Search(
            [FromQuery] string? name,
            [FromQuery] string? description,
            [FromQuery] int page = 1,
            [FromQuery] int count = 10)
        {
            try
            {
                List<FoodResponseDto> search = _service.Search(name, description, null, page, count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
