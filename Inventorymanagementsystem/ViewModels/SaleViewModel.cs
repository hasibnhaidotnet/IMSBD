using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace InventorymanagementSystem.ViewModels
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Memo No")]
        public string MemoNo { get; set; }

        [Display(Name = "Sale Date")]
        [DataType(DataType.Date)]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }

        public List<SaleItemViewModel> SaleItems { get; set; } = new();

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Paid amount must be >= 0")]
        public decimal PaidAmount { get; set; }
        // Hidden JSON field for binding SaleItems from JS
        //public string SaleItemsJson
        //{
        //get => JsonSerializer.Serialize(SaleItems);
        //    set => SaleItems = string.IsNullOrWhiteSpace(value) ? new List<SaleItemViewModel>() : JsonSerializer.Deserialize<List<SaleItemViewModel>>(value);
        // }
        public IEnumerable<SelectListItem> ClientList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ProductList { get; set; } = new List<SelectListItem>();
    }
}
