using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Brand { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Product Code (SKU)")]
        public string ProductCode { get; set; }

        [Required]
        public int UnitId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [Display(Name = "Purchase Price")]
        public double PurchasePrice { get; set; }

        [Display(Name = "Selling Price")]
        public double SellingPrice { get; set; }

        [Display(Name = "Product Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Minimum Stock Level")]
        public int MinimumStockAlert { get; set; } = 0;

        public IEnumerable<SelectListItem>? UnitList { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<SelectListItem>? SubCategoryList { get; set; }
    }

}
