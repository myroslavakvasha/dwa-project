using AutoMapper;
using BL.DTOs.Food;
using BL.DTOs.Order;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using WebApp.ViewModels.Menu;
using WebApp.ViewModels.Order;

namespace WebApp.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly FoodService _foodService;
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;

        public BasketController(FoodService foodService, OrderService orderService, IMapper mapper)
        {
            _foodService = foodService;
            _orderService = orderService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            try
            {
                var basket = GetBasket();
                BasketIndexVM basketIndexVM = new BasketIndexVM
                {
                    Items = basket,
                    Payments = new SelectList(new[] { "Cash", "Card" })
                };
                return View(basketIndexVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int foodId)
        {
            try
            {
                var basket = GetBasket();

                BasketItemVM? basketItemVM = basket.FirstOrDefault(x => x.FoodId == foodId);
                if (basketItemVM != null)
                {
                    basketItemVM.Quantity++;
                }
                else
                {
                    var foodResponseDto = _foodService.GetById(foodId);
                    BasketItemVM add = new BasketItemVM
                    {
                        FoodId = foodId,
                        Name = foodResponseDto.Name,
                        Price = foodResponseDto.Price,
                        Quantity = 1,
                        ImageUrl = foodResponseDto.ImageUrl
                    };
                    basket.Add(add);
                }

                SaveBasket(basket);
                TempData["BasketMsg"] = "Item added to basket!";
                return RedirectToAction("Details", "Menu", new { id = foodId });
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int foodId)
        {
            try
            {
                var basket = GetBasket();
                basket.Remove(basket.FirstOrDefault(x => x.FoodId.Equals(foodId))!);
                SaveBasket(basket);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(BasketIndexVM basketVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    basketVM.Items = GetBasket();
                    basketVM.Payments = new SelectList(new[] { "Cash", "Card" });
                    return View("Index", basketVM);
                }

                var basket = GetBasket();
                var orderRequestDto = new OrderRequestDto
                {
                    PaymentType = basketVM.PaymentType,
                    Comment = basketVM.Comment,
                    OrderItems = basket.Select(x => new OrderItemRequestDto
                    {
                        FoodId = x.FoodId,
                        Quantity = x.Quantity
                    }).ToList()
                };

                var ordered = _orderService.CreateOrder(orderRequestDto, User.Identity?.Name!);
                HttpContext.Session.Remove("basket");

                return RedirectToAction("Confirmation", new { id = ordered.Id });
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        public ActionResult Confirmation(int id)
        {
            try
            {
                var orderDetailResponseDto = _orderService.GetById(id, User.Identity?.Name!);
                var orderDetailVM = new OrderDetailVM
                {
                    Id = orderDetailResponseDto.Id,
                    Username = User.Identity?.Name!,
                    Date = orderDetailResponseDto.Date,
                    Comment = orderDetailResponseDto.Comment,
                    OrderItems = orderDetailResponseDto.OrderItems.Select(x => _mapper.Map<OrderItemVM>(x)).ToList(),
                    PaymentType = orderDetailResponseDto.PaymentType
                };
                return View(orderDetailVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        private List<BasketItemVM> GetBasket()
        {
            var json = HttpContext.Session.GetString("basket");
            if (json == null) return new List<BasketItemVM>();
            return JsonSerializer.Deserialize<List<BasketItemVM>>(json)!;
        }

        private void SaveBasket(List<BasketItemVM> basket)
        {
            HttpContext.Session.SetString("basket", JsonSerializer.Serialize(basket));
        }
    }
}
