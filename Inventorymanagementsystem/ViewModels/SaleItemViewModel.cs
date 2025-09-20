using System.ComponentModel.DataAnnotations;

namespace InventorymanagementSystem.ViewModels
{
    public class SaleItemViewModel
    {
        public int ProductId { get; set; }


        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be positive")]
        public double Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal SellingPrice { get; set; }
    }
}
