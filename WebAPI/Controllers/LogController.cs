using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs.Log;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogService _service;

        public LogController(LogService service)
        {
            _service = service;
        }

        [HttpGet("get/{n}")]
        public ActionResult<List<LogResponseDto>> GetLastNRows(int n)
        {
            try
            {
                return Ok(_service.GetLogs(n));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("count")]
        public ActionResult<int> GetCount()
        {
            try
            {
                return _service.GetLogCount();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
