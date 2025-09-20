using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Brand { get; set; }

        [Required, StringLength(50)]
        public string ProductCode { get; set; }
        public double LowStockThreshold { get; set; } 

        [Required]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public double PurchasePrice { get; set; }
        public double SellingPrice { get; set; }

        public string? Notes { get; set; }

        public int MinimumStockAlert { get; set; } = 0;

        public ICollection<PurchaseItem>? PurchaseItems { get; set; }
        public ICollection<SaleItem>? SaleItems { get; set; }
    }

}
