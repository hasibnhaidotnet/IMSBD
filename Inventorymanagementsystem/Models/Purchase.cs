using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Inventorymanagementsystem.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required]
        public string MemoNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public int ClientId { get; set; } // Supplier
        public Client Client { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }  // User কত টাকা দিয়েছে

        public ICollection<PurchaseItem>? PurchaseItems { get; set; } = new List<PurchaseItem>();

        public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();

        // Total Amount (for View/Print)
        [Display(Name = "Total Amount")]
        public decimal TotalAmount => PurchaseItems?.Sum(item => (decimal)item.PurchasePrice * (decimal)item.Quantity) ?? 0;

        // Due Amount Calculation - Total - Paid
        [NotMapped]
        public decimal DueAmount => TotalAmount - PaidAmount;
    }
}
