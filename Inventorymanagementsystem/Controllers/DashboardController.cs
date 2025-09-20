using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Inventorymanagementsystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            double lowStockThreshold = 10;

            var productStocks = _context.Products
                .Include(p => p.Unit)
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Select(p => new ProductStockViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    UnitName = p.Unit.Name,
                    CategoryName = p.Category.Name,
                    SubCategoryName = p.SubCategory.Name,

                    TotalPurchased = _context.PurchaseItems
                        .Where(pi => pi.ProductId == p.Id)
                        .Sum(pi => (double?)pi.Quantity) ?? 0,

                    TotalPurchaseReturn = _context.PurchaseReturns
                        .Where(pr => pr.PurchaseItem.ProductId == p.Id)
                        .Sum(pr => (double?)pr.Quantity) ?? 0,

                    TotalSold = _context.SaleItems
                        .Where(si => si.ProductId == p.Id)
                        .Sum(si => (double?)si.Quantity) ?? 0,

                    TotalSalesReturn = _context.SalesReturns
                        .Where(sr => sr.SaleItem.ProductId == p.Id)
                        .Sum(sr => (double?)sr.Quantity) ?? 0,

                    TotalDamaged = _context.Damages
                        .Where(d => d.ProductId == p.Id)
                        .Sum(d => (double?)d.Quantity) ?? 0
                })
                .ToList();

            var lowStockProducts = productStocks
                .Where(p => p.CurrentStock <= lowStockThreshold)
                .ToList();

            return View(lowStockProducts);
        }
    }
}
