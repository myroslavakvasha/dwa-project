using DAL.Models;
using Microsoft.EntityFrameworkCore;
using DAL.DTOs.Order;

namespace DAL.Services
{
    public class OrderService
    {
        private readonly GrillPizzaOrdersContext _context;
        private readonly LogService _logService;

        public OrderService(GrillPizzaOrdersContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public List<OrderResponseDto> GetAllOrders()
        {
            return 
                _context.Orders
                .Include(x => x.Payment)
                .Select(x => new OrderResponseDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    UserId = x.UserId,
                    PaymentType = x.Payment.Type,
                    Comment = x.Comment
                })
                .ToList();
        }

        public List<OrderResponseDto> GetByUsername(string username)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                _logService.LogAction("ERROR", $"No user with username={username} exists.");
                throw new Exception("Wrong username");
            }

            return
                _context.Orders
                .Include(x => x.Payment)
                .Where(x => x.UserId == user.Id)
                .Select(x => new OrderResponseDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    UserId = x.UserId,
                    PaymentType = x.Payment.Type,
                    Comment = x.Comment
                })
                .ToList();
        }

        public OrderDetailResponseDto GetById(int id, string username)
        {
            Order? order = 
                _context.Orders
                .Include(x => x.Payment)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Food)
                .FirstOrDefault(x => x.Id == id);

            if(order == null)
            {
                _logService.LogAction("ERROR", $"Cannot find order with id={id}");
                throw new InvalidOperationException("No order with such Id exists");
            }

            User? user = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username == username);
            if(user == null)
            {
                _logService.LogAction("ERROR", $"No user with username={username} exists.");
                throw new Exception("Wrong username");
            }

            bool isAdmin = user.Role.RoleTitle == "Admin";

            if((order.UserId != user.Id) && !isAdmin)
            {
                _logService.LogAction("ERROR", $"User with id={user.Id} is not authorized to access order data.");
                throw new AccessViolationException("User not authorized to access resource");
            }

            return new OrderDetailResponseDto
            {
                Id = order.Id,
                Date = order.Date,
                UserId = order.UserId,
                PaymentType = order.Payment.Type,
                Comment = order.Comment,
                OrderItems = order.OrderItems
                .Select(x => new OrderItemResponseDto
                {
                    Id = x.Id,
                    FoodName = x.Food.Name,
                    TotalPrice = x.TotalPrice,
                    Quantity = x.Quantity
                })
                .ToList()
            };
        }

        public OrderDetailResponseDto CreateOrder(OrderRequestDto createdOrder, string username)
        {
            Payment? payment = _context.Payments.FirstOrDefault(x => x.Type.ToLower() == createdOrder.PaymentType.ToLower());
            if (payment == null)
            {
                _logService.LogAction("ERROR", $"Cannot create order (payment type {createdOrder.PaymentType} doesn't exist).");
                throw new Exception("No payment type with such name exists");
            }

            User? user = _context.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                _logService.LogAction("ERROR", $"Cannot create order for user with username={username} (doesn't exist).");
                throw new Exception("No user with such Id exists");
            }

            var foodIds = createdOrder.OrderItems.Select(x => x.FoodId).ToList();
            var foods = _context.Foods.Where(x => foodIds.Contains(x.Id)).ToList();

            if(foods.Count != foodIds.Count)
            {
                _logService.LogAction("ERROR", $"Cannot create order (one or more food Ids don't exist).");
                throw new Exception("One or more food items don't exist");
            }

            Order order = new Order
            {
                Date = DateTime.Now,
                UserId = user.Id,
                PaymentId = payment.Id,
                Comment = createdOrder.Comment,
                OrderItems = createdOrder.OrderItems
                    .Select(x => {
                        var food = foods.FirstOrDefault(y => y.Id == x.FoodId);
                        return new OrderItem
                        {
                            FoodId = x.FoodId,
                            Quantity = x.Quantity,
                            TotalPrice = food!.Price * x.Quantity
                        };
                     })
                    .ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Order with id={order.Id} has been created");

            return new OrderDetailResponseDto
            {
                Id = order.Id,
                Date = order.Date,
                UserId = order.UserId,
                PaymentType = payment.Type,
                Comment = order.Comment,
                OrderItems = order.OrderItems
                    .Select(x => new OrderItemResponseDto
                    {
                        Id = x.Id,
                        FoodName = foods.FirstOrDefault(y => y.Id == x.FoodId)!.Name,
                        Quantity = x.Quantity,
                        TotalPrice = x.TotalPrice
                    })
                    .ToList()
            };
        }

        public void DeleteOrder(int id, string username)
        {
            Order? order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if(order == null)
            {
                _logService.LogAction("ERROR", $"Cannot delete order with id={id} (doesn't exist).");
                throw new InvalidOperationException("No order with such Id exists");
            }

            User? user = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                _logService.LogAction("ERROR", $"No user with username={username} exists.");
                throw new Exception("Wrong username");
            }

            bool isAdmin = user.Role.RoleTitle == "Admin";

            if ((order.UserId != user.Id) && !isAdmin)
            {
                _logService.LogAction("ERROR", $"User with id={user.Id} is not authorized to modify order with id={order.Id}.");
                throw new AccessViolationException("User not authorized to modify resource");
            }

            List<OrderItem> orderItems = _context.OrderItems.Where(x => x.OrderId == id).ToList();

            _context.OrderItems.RemoveRange(orderItems);
            _context.Remove(order);
            _context.SaveChanges();

            _logService.LogAction("INFO", $"Order with id={id} has been deleted");
        }
    }
}
