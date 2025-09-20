using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventorymanagementsystem.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Memo No")]
        public string MemoNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal PaidAmount { get; set; }  // User কত টাকা দিয়েছে
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        [NotMapped]
        public decimal DueAmount => TotalAmount - PaidAmount;
        // Navigation to Sale Items
        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

        // Total Amount (for View/Print)
        [Display(Name = "Total Amount")]
        public decimal TotalAmount => SaleItems?.Sum(item => item.SellingPrice * (decimal)item.Quantity) ?? 0;
    }
}
