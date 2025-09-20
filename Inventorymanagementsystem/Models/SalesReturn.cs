using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventorymanagementsystem.Models
{
    public class SalesReturn
    {
        public int Id { get; set; }

        [Required]
        public int SaleItemId { get; set; }

        [ForeignKey("SaleItemId")]
        public SaleItem SaleItem { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Return quantity must be greater than zero.")]
        public double Quantity { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Note { get; set; }
    }
}
