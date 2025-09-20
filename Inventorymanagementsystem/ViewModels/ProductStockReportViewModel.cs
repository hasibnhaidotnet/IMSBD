namespace Inventorymanagementsystem.ViewModels
{
    public class ProductStockReportViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

        public double TotalPurchased { get; set; }
        public double TotalSold { get; set; }
        public double PurchaseReturned { get; set; } // Out from stock
        public double SalesReturned { get; set; } // In to stock
        public double Damaged { get; set; }

        public double CurrentStock => (TotalPurchased + SalesReturned) - (TotalSold + PurchaseReturned + Damaged);
    }
}
