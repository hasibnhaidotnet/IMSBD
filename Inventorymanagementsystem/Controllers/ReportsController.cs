//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Inventorymanagementsystem.Models; // Adjust namespace
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Inventorymanagementsystem.Data;
//using Inventorymanagementsystem.ViewModels;

//public class MemoReportController : Controller
//{
//    private readonly ApplicationDbContext _context;

//    public MemoReportController(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    // Main report view with filtering and export option
//    public IActionResult MemoReport(DateTime? fromDate, DateTime? toDate, string? export)
//    {
//        if (fromDate == null || toDate == null)
//        {
//            fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
//            toDate = fromDate.Value.AddMonths(1).AddDays(-1);
//        }

//        var allMemos = new List<MemoReportViewModel>();
//        var productStock = new Dictionary<string, double>();

//        // Load Purchases
//        var purchases = _context.Purchases
//            .Include(p => p.Client)
//            .Include(p => p.PurchaseItems)
//                .ThenInclude(i => i.Product)
//            .Where(p => p.PurchaseDate >= fromDate && p.PurchaseDate <= toDate)
//            .Select(p => new
//            {
//                MemoId = p.Id,
//                Date = p.PurchaseDate,
//                MemoNo = p.MemoNo,
//                ClientName = p.Client.Name,
//                ClientType = p.Client.ClientType,
//                TotalAmount = p.PurchaseItems.Sum(i => i.Quantity * i.PurchasePrice),
//                Items = p.PurchaseItems.Select(i => new
//                {
//                    ProductName = i.Product.Name,
//                    Quantity = i.Quantity
//                }).ToList()
//            }).ToList();

//        // Load Sales
//        var sales = _context.Sales
//            .Include(s => s.Client)
//            .Include(s => s.SaleItems)
//                .ThenInclude(i => i.Product)
//            .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
//            .Select(s => new
//            {
//                MemoId = s.Id,
//                Date = s.SaleDate,
//                MemoNo = s.MemoNo,
//                ClientName = s.Client.Name,
//                ClientType = s.Client.ClientType,
//                TotalAmount = s.SaleItems.Sum(i => (double)i.Quantity * (double)i.SellingPrice),
//                Items = s.SaleItems.Select(i => new
//                {
//                    ProductName = i.Product.Name,
//                    Quantity = i.Quantity
//                }).ToList()
//            }).ToList();

//        // Merge and order
//        var combinedMemos = purchases.Concat(sales)
//            .OrderBy(m => m.Date)
//            .ToList();

//        // Calculate stock after each memo
//        foreach (var memo in combinedMemos)
//        {
//            foreach (var item in memo.Items)
//            {
//                if (!productStock.ContainsKey(item.ProductName))
//                    productStock[item.ProductName] = 0;

//                if (memo.ClientType == ClientType.Supplier)
//                    productStock[item.ProductName] += item.Quantity;
//                else if (memo.ClientType == ClientType.Buyer)
//                    productStock[item.ProductName] -= item.Quantity;
//            }

//            allMemos.Add(new MemoReportViewModel
//            {
//                MemoId = memo.MemoId,
//                Date = memo.Date,
//                MemoNumber = memo.MemoNo,
//                ClientName = memo.ClientName,
//                ClientType = memo.ClientType,
//                TotalAmount = (decimal)memo.TotalAmount,
//                ProductStocksAfterMemo = new Dictionary<string, double>(productStock)
//            });
//        }

//        // Totals
//        var totalPurchase = allMemos
//            .Where(m => m.ClientType == ClientType.Supplier)
//            .Sum(m => m.TotalAmount);

//        var totalSales = allMemos
//            .Where(m => m.ClientType == ClientType.Buyer)
//            .Sum(m => m.TotalAmount);

//        var viewModel = new MemoReportSummaryViewModel
//        {
//            FromDate = fromDate,
//            ToDate = toDate,
//            Memos = allMemos,
//            TotalPurchase = totalPurchase,
//            TotalSales = totalSales
//        };

//        if (!string.IsNullOrEmpty(export))
//        {
//            if (export.ToLower() == "pdf")
//            {
//                // TODO: Add your PDF export logic here (e.g., Rotativa or any library)
//                return Content("PDF Export not implemented yet.");
//            }
//            else if (export.ToLower() == "print")
//            {
//                // Return print-friendly view
//                return View("MemoReportPrint", viewModel);
//            }
//        }

//        return View(viewModel);
//    }

//    // Print View (A4 size optimized)
//    public IActionResult MemoReportPrint(DateTime? fromDate, DateTime? toDate)
//    {
//        // Same logic can be moved to a private method to avoid repetition, but repeating here for clarity

//        if (fromDate == null || toDate == null)
//        {
//            fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
//            toDate = fromDate.Value.AddMonths(1).AddDays(-1);
//        }

//        var allMemos = new List<MemoReportViewModel>();
//        var productStock = new Dictionary<string, double>();

//        var purchases = _context.Purchases
//            .Include(p => p.Client)
//            .Include(p => p.PurchaseItems)
//                .ThenInclude(i => i.Product)
//            .Where(p => p.PurchaseDate >= fromDate && p.PurchaseDate <= toDate)
//            .Select(p => new
//            {
//                MemoId = p.Id,
//                Date = p.PurchaseDate,
//                MemoNo = p.MemoNo,
//                ClientName = p.Client.Name,
//                ClientType = p.Client.ClientType,
//                TotalAmount = p.PurchaseItems.Sum(i => i.Quantity * i.PurchasePrice),
//                Items = p.PurchaseItems.Select(i => new
//                {
//                    ProductName = i.Product.Name,
//                    Quantity = i.Quantity
//                }).ToList()
//            }).ToList();

//        var sales = _context.Sales
//            .Include(s => s.Client)
//            .Include(s => s.SaleItems)
//                .ThenInclude(i => i.Product)
//            .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
//            .Select(s => new
//            {
//                MemoId = s.Id,
//                Date = s.SaleDate,
//                MemoNo = s.MemoNo,
//                ClientName = s.Client.Name,
//                ClientType = s.Client.ClientType,
//                TotalAmount = s.SaleItems.Sum(i => (double)i.Quantity *(double) i.SellingPrice),
//                Items = s.SaleItems.Select(i => new
//                {
//                    ProductName = i.Product.Name,
//                    Quantity = i.Quantity
//                }).ToList()
//            }).ToList();

//        var combinedMemos = purchases.Concat(sales)
//            .OrderBy(m => m.Date)
//            .ToList();

//        foreach (var memo in combinedMemos)
//        {
//            foreach (var item in memo.Items)
//            {
//                if (!productStock.ContainsKey(item.ProductName))
//                    productStock[item.ProductName] = 0;

//                if (memo.ClientType == ClientType.Supplier)
//                    productStock[item.ProductName] += item.Quantity;
//                else if (memo.ClientType == ClientType.Buyer)
//                    productStock[item.ProductName] -= item.Quantity;
//            }

//            allMemos.Add(new MemoReportViewModel
//            {
//                MemoId = memo.MemoId,
//                Date = memo.Date,
//                MemoNumber = memo.MemoNo,
//                ClientName = memo.ClientName,
//                ClientType = memo.ClientType,
//                TotalAmount = (decimal)memo.TotalAmount,
//                ProductStocksAfterMemo = new Dictionary<string, double>(productStock)
//            });
//        }

//        var totalPurchase = allMemos
//            .Where(m => m.ClientType == ClientType.Supplier)
//            .Sum(m => m.TotalAmount);

//        var totalSales = allMemos
//            .Where(m => m.ClientType == ClientType.Buyer)
//            .Sum(m => m.TotalAmount);

//        var viewModel = new MemoReportSummaryViewModel
//        {
//            FromDate = fromDate,
//            ToDate = toDate,
//            Memos = allMemos,
//            TotalPurchase = totalPurchase,
//            TotalSales = totalSales
//        };

//        return View(viewModel);
//    }
//}
