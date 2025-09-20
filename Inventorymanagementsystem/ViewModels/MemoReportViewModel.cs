using Inventorymanagementsystem.Models;

namespace Inventorymanagementsystem.ViewModels
{
    public class MemoReportViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<MemoViewModel> Memos { get; set; } = new();
        public double TotalPurchase { get; set; }
        public double TotalSales { get; set; }
    }

    public class MemoViewModel
    {
        public string MemoNo { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public string MemoType { get; set; } // "Purchase" or "Sales"
        public List<MemoProductItem> Items { get; set; } = new();
        public decimal Subtotal => Items.Sum(x => x.Total);
    }

    public class MemoProductItem
    {
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitName { get; set; }
        public decimal Total => (decimal)Quantity * UnitPrice;
    }
}
