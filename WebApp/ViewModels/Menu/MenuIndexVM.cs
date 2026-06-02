using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Food;

namespace WebApp.ViewModels.Menu
{
    public class MenuIndexVM
    {
        public string Q { get; set; } = "";
        public int CategoryId { get; set; }
        public SelectList Categories { get; set; }
        public int Page { get; set; } = 1;
        public int LastPage { get; set; }
        public IEnumerable<FoodRowVM> Foods { get; set; }
    }
}
