using Inventorymanagementsystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a client")]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select transaction type")]
        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        public string? Note { get; set; }

        public List<SelectListItem>? Clients { get; set; }
    }
}
