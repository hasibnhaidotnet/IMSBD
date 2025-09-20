using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventorymanagementsystem.ViewModel
{
    public class ProductStockFilterViewModel
    {
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public bool? ShowLowStockOnly { get; set; }

        public List<SelectListItem>? Categories { get; set; }
        public List<SelectListItem>? SubCategories { get; set; }

        public List<ProductStockViewModel>? ProductStocks { get; set; }
    }
}
