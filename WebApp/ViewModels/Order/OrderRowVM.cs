using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Order
{
    public class OrderRowVM
    {
        [Display(Name = "Order id")]
        public int Id { get; set; }

        [Display(Name = "Order date")]
        public DateTime Date { get; set; }

        [Display(Name = "Payment type")]
        public string? PaymentType { get; set; }

        [Display(Name = "Comment")]
        public string? Comment { get; set; }
    }
}
