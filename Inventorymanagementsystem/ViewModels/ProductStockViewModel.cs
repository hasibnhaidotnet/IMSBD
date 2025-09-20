namespace Inventorymanagementsystem.ViewModel
{
    public class ProductStockViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

        public double TotalPurchased { get; set; }
        public double TotalPurchaseReturn { get; set; } // In
        public double TotalSold { get; set; }
        public double TotalSalesReturn { get; set; }     // Out
        public double TotalDamaged { get; set; }

        public double CurrentStock =>
            (TotalPurchased + TotalSalesReturn) - (TotalSold + TotalPurchaseReturn + TotalDamaged);
    }
}
