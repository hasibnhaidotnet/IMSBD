using Inventorymanagementsystem.ViewModel;

namespace Inventorymanagementsystem.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalSales { get; set; }
        public int TotalDamaged { get; set; }
        public int TotalReturns { get; set; }
        public double TodaySalesAmount { get; set; }

        public List<ProductStockViewModel> LowStockProducts { get; set; } = new();
    }

}
