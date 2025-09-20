using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Inventorymanagementsystem.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Client)
                .Include(p => p.PurchaseItems)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();

            var purchaseWithPayments = purchases.Select(purchase => new PurchaseWithPaymentViewModel
            {
                Purchase = purchase,
                TotalAmount = purchase.PurchaseItems.Sum(i => (decimal)i.Quantity *(decimal) i.PurchasePrice),
                PaidAmount = _context.Transactions
                    .Where(t => t.PurchaseId == purchase.Id && t.TransactionType == TransactionType.Payment)
                    .Sum(t => t.Amount)
            }).ToList();

            return View(purchaseWithPayments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var purchase = await _context.Purchases
                .Include(p => p.Client)
                .Include(p => p.PurchaseItems).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchase == null) return NotFound();

            return View(purchase);
        }

        public IActionResult Create()
        {
            var model = new PurchaseViewModel
            {
                MemoNo = GeneratePurchaseMemoNo(),
                PurchaseDate = DateTime.Today,
                ClientList = GetSupplierSelectList(),
                ProductList = GetProductSelectList(),
                Items = new List<PurchaseItemViewModel> { new() },
                PaidAmount = 0m
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ClientList = GetSupplierSelectList();
                model.ProductList = GetProductSelectList();
                return View(model);
            }

            var purchase = new Purchase
            {
                MemoNo = model.MemoNo,
                PurchaseDate = model.PurchaseDate,
                ClientId = model.ClientId,
                PaidAmount = model.PaidAmount,
                PurchaseItems = model.Items.Select(i => new PurchaseItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    PurchasePrice = i.PurchasePrice
                }).ToList()
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            var transaction = new Transaction
            {
                ClientId = purchase.ClientId,
                TransactionType = TransactionType.Payment,
                Amount = purchase.PaidAmount,
                TransactionDate = purchase.PurchaseDate,
                Note = $"Auto entry from Purchase Memo: {purchase.MemoNo}",
                PurchaseId = purchase.Id
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Purchase/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null) return NotFound();

            var model = new PurchaseViewModel
            {
                Id = purchase.Id,
                MemoNo = purchase.MemoNo,
                PurchaseDate = purchase.PurchaseDate,
                ClientId = purchase.ClientId,
                PaidAmount = purchase.PaidAmount,
                Items = purchase.PurchaseItems.Select(i => new PurchaseItemViewModel
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    PurchasePrice = i.PurchasePrice
                }).ToList(),
                ClientList = GetSupplierSelectList(),
                ProductList = GetProductSelectList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ClientList = GetSupplierSelectList();
                model.ProductList = GetProductSelectList();
                return View(model);
            }

            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null) return NotFound();

            purchase.MemoNo = model.MemoNo;
            purchase.PurchaseDate = model.PurchaseDate;
            purchase.ClientId = model.ClientId;
            purchase.PaidAmount = model.PaidAmount;

            _context.PurchaseItems.RemoveRange(purchase.PurchaseItems);
            purchase.PurchaseItems = model.Items.Select(i => new PurchaseItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                PurchasePrice = i.PurchasePrice,
                PurchaseId = purchase.Id
            }).ToList();

            await _context.SaveChangesAsync();

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.PurchaseId == purchase.Id);

            if (transaction != null)
            {
                transaction.ClientId = purchase.ClientId;
                transaction.Amount = purchase.PaidAmount;
                transaction.TransactionDate = purchase.PurchaseDate;
                transaction.Note = $"Auto entry from Purchase Memo: {purchase.MemoNo}";
            }
            else
            {
                transaction = new Transaction
                {
                    ClientId = purchase.ClientId,
                    TransactionType = TransactionType.Payment,
                    Amount = purchase.PaidAmount,
                    TransactionDate = purchase.PurchaseDate,
                    Note = $"Auto entry from Purchase Memo: {purchase.MemoNo}",
                    PurchaseId = purchase.Id
                };
                _context.Transactions.Add(transaction);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var purchase = await _context.Purchases
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null) return NotFound();

            return View(purchase);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase != null)
            {
                var transaction = await _context.Transactions
                    .FirstOrDefaultAsync(t => t.PurchaseId == purchase.Id);

                if (transaction != null)
                {
                    _context.Transactions.Remove(transaction);
                }

                _context.PurchaseItems.RemoveRange(purchase.PurchaseItems);
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        public JsonResult GetProductList()
        {
            var productList = _context.Products
                .Select(p => new { p.Id, p.Name })
                .ToList();

            return Json(productList);
        }

        private IEnumerable<SelectListItem> GetSupplierSelectList()
        {
            return _context.Clients
                .Where(c => c.ClientType == ClientType.Supplier)
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
        private string GeneratePurchaseMemoNo()
        {
            var lastMemo = _context.Purchases
                .OrderByDescending(p => p.Id)
                .FirstOrDefault()?.MemoNo;

            int lastNumber = 0;

            if (!string.IsNullOrEmpty(lastMemo) && lastMemo.StartsWith("P-"))
            {
                int.TryParse(lastMemo.Substring(2), out lastNumber);
            }

            return $"P-{(lastNumber + 1).ToString("D4", CultureInfo.InvariantCulture)}";
        }

    }
}
