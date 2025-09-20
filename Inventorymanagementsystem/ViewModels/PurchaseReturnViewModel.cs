using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModel
{
    public class PurchaseReturnViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Purchase Item")]
        public int PurchaseItemId { get; set; }

        public string? SupplierName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public double Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        public string? UnitName { get; set; } // ✅ Optional for view
        public string? ProductName { get; set; } // ✅ Add this
        public IEnumerable<SelectListItem>? PurchaseItemList { get; set; }
    }
}
