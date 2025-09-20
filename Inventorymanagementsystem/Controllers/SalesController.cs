using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using InventorymanagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Inventorymanagementsystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleItems)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

            var salesWithPayments = sales.Select(sale => new SaleWithPaymentViewModel
            {
                Sale = sale,
                TotalAmount = sale.SaleItems.Sum(i => (decimal)i.Quantity * i.SellingPrice),
                PaidAmount = _context.Transactions
                    .Where(t => t.SaleId == sale.Id && t.TransactionType == TransactionType.Receive)
                    .Sum(t => t.Amount)
            }).ToList();

            return View(salesWithPayments);
        }

        public IActionResult Create()
        {
            var model = new SaleViewModel
            {
                MemoNo = GenerateMemoNo(),
                SaleDate = DateTime.Today,
                ClientList = GetBuyerSelectList(),
                ProductList = GetProductSelectList(),
                SaleItems = new List<SaleItemViewModel> { new() },
                PaidAmount = 0m
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ClientList = GetBuyerSelectList();
                model.ProductList = GetProductSelectList();
                return View(model);
            }

            var sale = new Sale
            {
                MemoNo = model.MemoNo,
                SaleDate = model.SaleDate,
                ClientId = model.ClientId,
                PaidAmount = model.PaidAmount,  
                SaleItems = model.SaleItems.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    SellingPrice = i.SellingPrice
                }).ToList()
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var transaction = new Transaction
            {
                ClientId = sale.ClientId,
                TransactionType = TransactionType.Receive, 
                Amount = sale.PaidAmount,  
                TransactionDate = sale.SaleDate,
                Note = $"Auto entry from Sale Memo: {sale.MemoNo}",
                SaleId = sale.Id
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sale = await _context.Sales
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return NotFound();

            var model = new SaleViewModel
            {
                Id = sale.Id,
                MemoNo = sale.MemoNo,
                SaleDate = sale.SaleDate,
                ClientId = sale.ClientId,
                PaidAmount = sale.PaidAmount,
                SaleItems = sale.SaleItems.Select(i => new SaleItemViewModel
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    SellingPrice = i.SellingPrice
                }).ToList(),
                ClientList = GetBuyerSelectList(),
                ProductList = GetProductSelectList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaleViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ClientList = GetBuyerSelectList();
                model.ProductList = GetProductSelectList();
                return View(model);
            }

            var sale = await _context.Sales
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return NotFound();

            sale.MemoNo = model.MemoNo;
            sale.SaleDate = model.SaleDate;
            sale.ClientId = model.ClientId;
            sale.PaidAmount = model.PaidAmount; 

            _context.SaleItems.RemoveRange(sale.SaleItems);

            sale.SaleItems = model.SaleItems.Select(i => new SaleItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                SellingPrice = i.SellingPrice,
                SaleId = sale.Id
            }).ToList();

            await _context.SaveChangesAsync();

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.SaleId == sale.Id);

            if (transaction != null)
            {
                transaction.ClientId = sale.ClientId;
                transaction.Amount = sale.PaidAmount;
                transaction.TransactionDate = sale.SaleDate;
                transaction.Note = $"Auto entry from Sale Memo: {sale.MemoNo}";
            }
            else
            {
                transaction = new Transaction
                {
                    ClientId = sale.ClientId,
                    TransactionType = TransactionType.Receive,
                    Amount = sale.PaidAmount,
                    TransactionDate = sale.SaleDate,
                    Note = $"Auto entry from Sale Memo: {sale.MemoNo}",
                    SaleId = sale.Id
                };
                _context.Transactions.Add(transaction);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sale = await _context.Sales
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return NotFound();

            return View(sale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale != null)
            {
                var transaction = await _context.Transactions
                    .FirstOrDefaultAsync(t => t.SaleId == sale.Id);

                if (transaction != null)
                {
                    _context.Transactions.Remove(transaction);
                }

                _context.SaleItems.RemoveRange(sale.SaleItems);
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sale == null) return NotFound();

            var viewModel = new SaleViewModel
            {
                Id = sale.Id,
                MemoNo = sale.MemoNo,
                SaleDate = sale.SaleDate,
                ClientId = sale.ClientId,
                PaidAmount = sale.PaidAmount,
                SaleItems = sale.SaleItems.Select(si => new SaleItemViewModel
                {
                    ProductId = si.ProductId,
                    Quantity = si.Quantity,
                    SellingPrice = si.SellingPrice
                }).ToList(),
                ClientList = await _context.Clients
                    .Where(c => c.ClientType == ClientType.Buyer)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToListAsync(),
                ProductList = await _context.Products
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToListAsync()
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Print(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleItems).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return NotFound();

            return View(sale);
        }

        public JsonResult GetProductList()
        {
            var productList = _context.Products
                .Select(p => new { p.Id, p.Name })
                .ToList();

            return Json(productList);
        }

        private IEnumerable<SelectListItem> GetBuyerSelectList()
        {
            return _context.Clients
                .Where(c => c.ClientType == ClientType.Buyer)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
        }

        private List<SelectListItem> GetProductSelectList()
        {
            return _context.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
        }

        private string GenerateMemoNo()
        {
            var lastMemo = _context.Sales.OrderByDescending(p => p.Id).FirstOrDefault()?.MemoNo;
            int lastNumber = 0;

            if (!string.IsNullOrEmpty(lastMemo) && lastMemo.StartsWith("S-"))
            {
                int.TryParse(lastMemo.Substring(2), out lastNumber);
            }

            return $"S-{(lastNumber + 1).ToString("D4", CultureInfo.InvariantCulture)}";
        }
    }
}
