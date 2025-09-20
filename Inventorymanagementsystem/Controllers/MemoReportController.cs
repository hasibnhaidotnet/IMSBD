using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModel;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class MemoReportController : Controller
{
    private readonly ApplicationDbContext _context;

    public MemoReportController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(DateTime? fromDate, DateTime? toDate)
    {
        var vm = new MemoReportViewModel
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        var purchaseQuery = _context.Purchases
            .Include(p => p.PurchaseItems).ThenInclude(pi => pi.Product).ThenInclude(p => p.Unit)
            .Include(p => p.Client)
            .AsQueryable();

        var salesQuery = _context.Sales
            .Include(s => s.SaleItems).ThenInclude(si => si.Product).ThenInclude(p => p.Unit)
            .Include(s => s.Client)
            .AsQueryable();

        if (fromDate.HasValue)
        {
            purchaseQuery = purchaseQuery.Where(x => x.PurchaseDate >= fromDate.Value);
            salesQuery = salesQuery.Where(x => x.SaleDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            purchaseQuery = purchaseQuery.Where(x => x.PurchaseDate <= toDate.Value);
            salesQuery = salesQuery.Where(x => x.SaleDate <= toDate.Value);
        }

        var purchaseMemos = purchaseQuery.ToList().Select(p => new MemoViewModel
        {
            MemoNo = p.MemoNo,
            Date = p.PurchaseDate,
            ClientName = p.Client?.Name,
            MemoType = "Purchase",
            Items = p.PurchaseItems.Select(i => new MemoProductItem
            {
                ProductName = i.Product?.Name,
                Quantity = i.Quantity,
                UnitPrice = (decimal)i.PurchasePrice,
                UnitName = i.Product?.Unit?.Name
            }).ToList()
        });

        var salesMemos = salesQuery.ToList().Select(s => new MemoViewModel
        {
            MemoNo = s.MemoNo,
            Date = s.SaleDate,
            ClientName = s.Client?.Name,
            MemoType = "Sales",
            Items = s.SaleItems.Select(i => new MemoProductItem
            {
                ProductName = i.Product?.Name,
                Quantity = i.Quantity,
                UnitPrice = i.SellingPrice,
                UnitName = i.Product?.Unit?.Name
            }).ToList()
        });

        vm.Memos = purchaseMemos.Concat(salesMemos)
            .OrderBy(m => m.Date)
            .ThenBy(m => m.MemoNo)
            .ToList();

        vm.TotalPurchase = (double)vm.Memos.Where(m => m.MemoType == "Purchase").Sum(m => m.Subtotal);
        vm.TotalSales = (double)vm.Memos.Where(m => m.MemoType == "Sales").Sum(m => m.Subtotal);
        return View(vm);
    }

    public IActionResult Print(DateTime? fromDate, DateTime? toDate)
    {
        return RedirectToAction("Index", new { fromDate, toDate });
    }
}
