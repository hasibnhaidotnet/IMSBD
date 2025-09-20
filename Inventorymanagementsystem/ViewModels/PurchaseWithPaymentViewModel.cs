using Inventorymanagementsystem.Models;

namespace Inventorymanagementsystem.ViewModels
{
    public class PurchaseWithPaymentViewModel
    {
        public Purchase Purchase { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
