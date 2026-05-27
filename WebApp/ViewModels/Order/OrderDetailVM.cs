using BL.DTOs.Order;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Order
{
    public class OrderDetailVM
    {
        [Display(Name = "Order id")]
        public int Id { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Order date")]
        public DateTime Date { get; set; }

        [Display(Name = "Payment type")]
        public string? PaymentType { get; set; }

        [Display(Name = "Comment")]
        public string? Comment { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }
    }
}
