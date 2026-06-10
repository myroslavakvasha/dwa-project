using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Menu
{
    public class BasketIndexVM
    {
        public List<BasketItemVM> Items { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentType { get; set; }

        [ValidateNever]
        public SelectList Payments { get; set; }

        public string? Comment { get; set; }

        public decimal TotalPrice => Items?.Sum(x => x.Price * x.Quantity) ?? 0;
    }
}
