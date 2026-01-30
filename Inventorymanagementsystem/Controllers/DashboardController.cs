using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            double lowStockThreshold = 10;

            var products = await _context.Products
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    UnitName = p.Unit.Name,
                    CategoryName = p.Category.Name,
                    SubCategoryName = p.SubCategory.Name
                })
                .ToListAsync();

            var purchase = await _context.PurchaseItems
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
                .ToListAsync();

            var purchaseReturn = await _context.PurchaseReturns
                .GroupBy(x => x.PurchaseItem.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
                .ToListAsync();

            var sales = await _context.SaleItems
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
                .ToListAsync();

            var salesReturn = await _context.SalesReturns
                .GroupBy(x => x.SaleItem.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
                .ToListAsync();

            var damage = await _context.Damages
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
                .ToListAsync();

            var productStocks = products.Select(p => new ProductStockViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                UnitName = p.UnitName,
                CategoryName = p.CategoryName,
                SubCategoryName = p.SubCategoryName,

                TotalPurchased = purchase.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0,
                TotalPurchaseReturn = purchaseReturn.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0,
                TotalSold = sales.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0,
                TotalSalesReturn = salesReturn.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0,
                TotalDamaged = damage.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0
            }).ToList();

            var lowStockProducts = productStocks
                .Where(p => p.CurrentStock <= lowStockThreshold)
                .ToList();

            return View(lowStockProducts);
        }
    }
}
