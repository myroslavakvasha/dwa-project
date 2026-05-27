using AutoMapper;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Order;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly OrderService _service;
        private readonly IMapper _mapper;

        public OrderController(OrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: OrderController
        public ActionResult Index(string username)
        {
            try
            {
                List<OrderRowVM> orderRowVMs = _service.GetByUsername(username).Select(_mapper.Map<OrderRowVM>).ToList();
                ViewBag.Username = username;
                return View(orderRowVMs);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id, string orderUsername)
        {
            try
            {
                string username = User.Identity.Name;
                OrderDetailVM orderDetailVM = _mapper.Map<OrderDetailVM>(_service.GetById(id, username));
                orderDetailVM.Username = orderUsername;
                return View(orderDetailVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
