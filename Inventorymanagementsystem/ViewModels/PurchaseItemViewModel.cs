using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class PurchaseItemViewModel
    {
        public int Id { get; set; } 
        public int PurchaseId { get; set; }
        public string? UnitName { get; set; } // ✅ Optional for view
        public string? ProductName { get; set; } // ✅ Add this

        [Required]
        public int ProductId { get; set; }

        public double Quantity { get; set; }

        [Display(Name = "Purchase Price")]
        public double PurchasePrice { get; set; }

        public IEnumerable<SelectListItem>? ProductList { get; set; }
    }

}
