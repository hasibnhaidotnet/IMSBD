using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class StockController : Controller
{
    private readonly ApplicationDbContext _context;

    public StockController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int? categoryId, int? subCategoryId, bool? showLowStockOnly)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.SubCategory)
            .Include(p => p.Unit)
            .ToList();

        var stockList = new List<ProductStockViewModel>();

        foreach (var product in products)
        {
            var purchase = _context.PurchaseItems.Where(x => x.ProductId == product.Id).ToList();
            var sales = _context.SaleItems.Where(x => x.ProductId == product.Id).ToList();
            var damage = _context.Damages.Where(x => x.ProductId == product.Id).ToList();
            var purchaseReturns = _context.PurchaseReturns.Where(x => x.PurchaseItem.ProductId == product.Id).ToList();
            var salesReturns = _context.SalesReturns.Where(x => x.SaleItem.ProductId == product.Id).ToList();

            var stock = new ProductStockViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitName = product.Unit?.Name,
                CategoryName = product.Category?.Name,
                SubCategoryName = product.SubCategory?.Name,
                TotalPurchased = purchase.Sum(x => x.Quantity),
                TotalSold = sales.Sum(x => x.Quantity),
                TotalDamaged = damage.Sum(x => x.Quantity),
                TotalPurchaseReturn = purchaseReturns.Sum(x => x.Quantity),
                TotalSalesReturn = salesReturns.Sum(x => x.Quantity)
            };

            stockList.Add(stock);
        }

        // Filtering Logic
        if (categoryId.HasValue)
            stockList = stockList.Where(x => x.CategoryName == _context.Categories.Find(categoryId)?.Name).ToList();

        if (subCategoryId.HasValue)
            stockList = stockList.Where(x => x.SubCategoryName == _context.SubCategories.Find(subCategoryId)?.Name).ToList();

        if (showLowStockOnly == true)
            stockList = stockList.Where(x => x.CurrentStock < 10).ToList(); // Threshold: 10

        var viewModel = new ProductStockFilterViewModel
        {
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            ShowLowStockOnly = showLowStockOnly,
            Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList(),
            SubCategories = _context.SubCategories
                .Select(sc => new SelectListItem { Value = sc.Id.ToString(), Text = sc.Name })
                .ToList(),
            ProductStocks = stockList
        };

        return View(viewModel);
    }
}
