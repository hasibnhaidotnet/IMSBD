using Inventorymanagementsystem.Models;

namespace Inventorymanagementsystem.ViewModels
{
    public class SaleWithPaymentViewModel
    {
        public Sale Sale { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount => TotalAmount - PaidAmount;
    }
}
