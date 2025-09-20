using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }

        [Required]
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }
        public double PurchasePrice { get; set; }

        public bool IsReturned { get; set; } = false;
        public bool IsDamaged { get; set; } = false;
    }
}
