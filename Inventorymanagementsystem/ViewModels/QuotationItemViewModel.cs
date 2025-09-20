using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class QuotationItemViewModel
    {
        [Required]
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public double Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal SellingPrice { get; set; }

        public List<SelectListItem> ProductList { get; set; } = new();
    }
}
