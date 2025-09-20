using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class QuotationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a client")]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [BindNever]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quotation date is required")]
        [Display(Name = "Quotation Date")]
        public DateTime QuotationDate { get; set; }

        public List<SelectListItem>? Clients { get; set; }
        public List<QuotationItemViewModel>? Items { get; set; } = new();
    }
}
