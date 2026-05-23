using BL.DTOs.Order;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<List<OrderResponseDto>> GetAll()
        {
            try
            {
                return Ok(_service.GetAllOrders());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<OrderDetailResponseDto> GetById(int id)
        {
            try
            {
                var username = User.Identity?.Name;
                return Ok(_service.GetById(id, username!));
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("myorders")]
        public ActionResult<List<OrderResponseDto>> GetMyOrders()
        {
            try
            {
                var username = User.Identity?.Name;
                return Ok(_service.GetByUsername(username!));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<OrderDetailResponseDto> Create([FromBody] OrderRequestDto createdOrder)
        {
            try
            {
                var username = User.Identity?.Name;
                OrderDetailResponseDto orderDetailResponseDto = _service.CreateOrder(createdOrder, username!);
                var uri = Url.Action(nameof(GetById));

                return Created(uri, orderDetailResponseDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var username = User.Identity?.Name;
                _service.DeleteOrder(id, username!);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
