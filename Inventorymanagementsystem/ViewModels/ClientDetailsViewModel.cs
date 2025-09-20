using Inventorymanagementsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class ClientDetailsViewModel
    {
        public Client Client { get; set; }
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public List<Sale> Sales { get; set; } = new List<Sale>();

        public decimal TotalPurchaseAmount { get; set; }
        public decimal TotalPurchasePaid { get; set; }
        public decimal PurchaseDue { get; set; }

        public decimal TotalSalesAmount { get; set; }
        public decimal TotalSalesPaid { get; set; }
        public decimal SalesDue { get; set; }

        public decimal TotalPaid { get; set; }
        public decimal TotalDue { get; set; }
    }
}
