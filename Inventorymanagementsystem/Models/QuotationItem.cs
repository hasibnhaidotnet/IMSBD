using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class QuotationItem
    {
        public int Id { get; set; }

        [Required]
        public int QuotationId { get; set; }
        public Quotation Quotation { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
