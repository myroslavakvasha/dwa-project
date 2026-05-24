using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Food
{
    public class FoodIndexVM
    {
        public string Q { get; set; } = "";
        public int CategoryId { get; set; }
        public SelectList Categories { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public int FromPager { get; set; }
        public int ToPager { get; set; }
        public int LastPage { get; set; }
        public IEnumerable<FoodRowVM> Foods { get; set; }
    }
}
