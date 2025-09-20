using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventorymanagementsystem.ViewModels
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Memo No")]
        public string MemoNo { get; set; }

        [Required]
        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Supplier")]
        public int ClientId { get; set; }
        [Display(Name = "Paid Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Paid amount cannot be negative")]
        public decimal PaidAmount { get; set; }
        public List<SelectListItem>? ProductList { get; set; }

        public List<PurchaseItemViewModel> Items { get; set; } = new();

        public IEnumerable<SelectListItem>? ClientList { get; set; }
    }

}
