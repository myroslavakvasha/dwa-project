using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.User;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AuthService _service;
        private readonly IMapper _mapper;

        public UserController(AuthService authService, IMapper mapper)
        {
            _service = authService;
            _mapper = mapper;
        }

        // GET: UserController
        public ActionResult Index()
        {
            try
            {
                List<UserRowVM> userRowVMs = _service.GetAllUsers().Select(_mapper.Map<UserRowVM>).ToList();
                return View(userRowVMs);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
