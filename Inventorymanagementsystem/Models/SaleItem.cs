using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class SaleItem
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public double Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Selling Price must be greater than 0.")]
        [Display(Name = "Unit Price")]
        public decimal SellingPrice { get; set; }

        // Optional PurchaseItem linkage for tracking cost
        [Display(Name = "Related Purchase")]
        public int? RelatedPurchaseItemId { get; set; }
        public PurchaseItem? RelatedPurchaseItem { get; set; }

        public bool IsReturned { get; set; } = false;

        // Derived Property for View/Report
        [Display(Name = "Total")]
        public decimal Total => (decimal)Quantity * SellingPrice;
        public virtual ICollection<SalesReturn> SalesReturns { get; set; } = new List<SalesReturn>();

    }
}
