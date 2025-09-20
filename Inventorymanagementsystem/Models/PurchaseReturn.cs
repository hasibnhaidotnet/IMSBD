using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventorymanagementsystem.Models
{
    public class PurchaseReturn
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        // Foreign Key
        [Required]
        public int PurchaseItemId { get; set; }

        [ForeignKey("PurchaseItemId")]
        public PurchaseItem PurchaseItem { get; set; }

    }
}
