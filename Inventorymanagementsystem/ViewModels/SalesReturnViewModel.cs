using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class SalesReturnViewModel
    {
        public int Id { get; set; }

        [Required]
        public int SaleItemId { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Return quantity must be greater than zero.")]
        public double Quantity { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Note { get; set; }

        [BindNever]
        public string ClientName { get; set; }

        [BindNever]
        public string MemoNo { get; set; }

        [BindNever]
        public string ProductName { get; set; }


        [BindNever]
        public DateTime SaleDate { get; set; }

        [BindNever]
        public double UnitPrice { get; set; }

        public double MaxReturnableQuantity { get; set; }

        public List<SelectListItem> ProductList { get; set; } = new List<SelectListItem>();
    }
}
