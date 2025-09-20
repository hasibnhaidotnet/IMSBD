//using Inventorymanagementsystem.Data;

//namespace Inventorymanagementsystem.Models
//{
//    public class MemoItem
//    {
//        public string MemoNo { get; set; }
//        public string MemoType { get; set; } // Purchase or Sales
//        public string ClientName { get; set; }
//        public DateTime Date { get; set; }
//        public List<MemoProductItem> ProductItems { get; set; } = new();
//    }
//    public class MemoProductItem
//    {
//        public string ProductName { get; set; }
//        public double Quantity { get; set; }
//        public decimal UnitPrice { get; set; }
//        public decimal Total { get; set; }
//    }
//    public class ProductStockSummary
//    {
//        public string ProductName { get; set; }
//        public string UnitName { get; set; }
//        public double CurrentStock { get; set; }
//    }
//    public static class StockCalculator
//    {
//        public static (double FinalStock) CalculateStock(ApplicationDbContext context, int productId)
//        {
//            double purchased = context.PurchaseItems.Where(p => p.ProductId == productId).Sum(p => p.Quantity);
//            double sold = context.SaleItems.Where(s => s.ProductId == productId).Sum(s => s.Quantity);
//            double damaged = context.Damages.Where(d => d.ProductId == productId).Sum(d => d.Quantity);
//            double purchaseReturn = context.PurchaseReturns.Where(r => r.PurchaseItem.ProductId == productId).Sum(r => r.Quantity);
//            double salesReturn = context.SalesReturns.Where(r => r.SaleItem.ProductId == productId).Sum(r => r.Quantity);

//            double finalStock = purchased - sold - damaged + purchaseReturn + salesReturn;
//            return (finalStock);
//        }
//    }

//}
