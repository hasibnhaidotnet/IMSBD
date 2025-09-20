using System;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class DamageViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        public string ProductName { get; set; } 

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public double Quantity { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Damage Date")]
        public DateTime DamageDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Note { get; set; }
    }
}
