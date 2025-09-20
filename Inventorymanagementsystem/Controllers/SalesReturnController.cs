using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class SalesReturnController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesReturnController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index
        public async Task<IActionResult> Index()
        {
            var salesReturns = await _context.SalesReturns
                .Include(sr => sr.SaleItem)
                    .ThenInclude(si => si.Product)
                .Include(sr => sr.SaleItem.Sale)
                    .ThenInclude(s => s.Client)
                .Select(sr => new SalesReturnViewModel
                {
                    Id = sr.Id,
                    ProductId = sr.SaleItem.ProductId,
                    ProductName = sr.SaleItem.Product.Name,
                    Quantity = sr.Quantity,
                    ReturnDate = sr.ReturnDate,
                    Note = sr.Note,
                    ClientName = sr.SaleItem.Sale.Client.Name,
                    MemoNo = sr.SaleItem.Sale.MemoNo
                })
                .ToListAsync();

            return View(salesReturns);
        }

        // Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sr = await _context.SalesReturns
                .Include(x => x.SaleItem)
                    .ThenInclude(si => si.Product)
                .Include(x => x.SaleItem.Sale)
                    .ThenInclude(s => s.Client)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (sr == null) return NotFound();

            var vm = new SalesReturnViewModel
            {
                Id = sr.Id,
                ProductId = sr.SaleItem.ProductId,
                ProductName = sr.SaleItem.Product.Name,
                Quantity = sr.Quantity,
                ReturnDate = sr.ReturnDate,
                Note = sr.Note,
                ClientName = sr.SaleItem.Sale.Client.Name,
                MemoNo = sr.SaleItem.Sale.MemoNo
            };

            return View(vm);
        }

        // Create
        public async Task<IActionResult> Create()
        {
            var products = await _context.SaleItems
                .Include(x => x.Product)
                .Include(x => x.Sale)
                .ThenInclude(c => c.Client)
                .Select(x => new
                {
                    x.Id,
                    Name = $"{x.Product.Name} - {x.Sale.MemoNo} ({x.Sale.Client.Name})",
                    x.Quantity,
                    Sold = x.Quantity,
                    Returned = _context.SalesReturns.Where(r => r.SaleItemId == x.Id).Sum(r => r.Quantity)
                }).ToListAsync();

            var model = new SalesReturnViewModel
            {
                ReturnDate = DateTime.Now,
                ProductList = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ProductList = await GetProductSelectListAsync();
                return View(model);
            }

            var saleItem = await _context.SaleItems
                .Include(x => x.Sale)
                .FirstOrDefaultAsync(x => x.Id == model.SaleItemId);

            if (saleItem == null)
            {
                ModelState.AddModelError("", "Invalid sale item.");
                model.ProductList = await GetProductSelectListAsync();
                return View(model);
            }

            var returnedQty = _context.SalesReturns
                .Where(x => x.SaleItemId == model.SaleItemId)
                .Sum(x => x.Quantity);

            var maxQty = saleItem.Quantity - returnedQty;

            if (model.Quantity > maxQty)
            {
                ModelState.AddModelError(nameof(model.Quantity), $"Only {maxQty} quantity is returnable.");
                model.ProductList = await GetProductSelectListAsync();
                return View(model);
            }

            var sr = new SalesReturn
            {
                SaleItemId = model.SaleItemId,
                Quantity = model.Quantity,
                ReturnDate = model.ReturnDate,
                Note = model.Note
            };

            _context.Add(sr);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Edit
        // GET: SalesReturn/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var salesReturn = await _context.SalesReturns
                .Include(r => r.SaleItem)
                    .ThenInclude(si => si.Product)
                .Include(r => r.SaleItem.Sale)
                    .ThenInclude(s => s.Client)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (salesReturn == null)
                return NotFound();

            var model = new SalesReturnViewModel
            {
                Id = salesReturn.Id,
                SaleItemId = salesReturn.SaleItemId,
                ProductId = salesReturn.SaleItem.ProductId,
                ProductName = salesReturn.SaleItem.Product.Name,
                Quantity = salesReturn.Quantity,
                ReturnDate = salesReturn.ReturnDate,
                Note = salesReturn.Note,
                MemoNo = salesReturn.SaleItem.Sale.MemoNo,
                ClientName = salesReturn.SaleItem.Sale.Client.Name,
                SaleDate = salesReturn.SaleItem.Sale.SaleDate,
                UnitPrice =(double) salesReturn.SaleItem.SellingPrice,

                // Max Returnable Quantity Calculation (optional logic)
                MaxReturnableQuantity = salesReturn.SaleItem.Quantity, // You may subtract already returned qty if needed

                ProductList = await _context.Products
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                    .ToListAsync()
            };

            return View(model);
        }


        // POST: SalesReturn/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesReturnViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            // Skip validation for display-only fields
            ModelState.Remove("MemoNo");
            ModelState.Remove("ClientName");
            ModelState.Remove("ProductName");

            if (ModelState.IsValid)
            {
                try
                {
                    var salesReturn = await _context.SalesReturns.FindAsync(id);
                    if (salesReturn == null)
                        return NotFound();

                    salesReturn.Quantity = model.Quantity;
                    salesReturn.ReturnDate = model.ReturnDate;
                    salesReturn.Note = model.Note;

                    _context.Update(salesReturn);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SalesReturns.Any(e => e.Id == model.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Reload dropdown if ModelState fails
            model.ProductList = await _context.Products
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToListAsync();

            return View(model);
        }

        // Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sr = await _context.SalesReturns
                .Include(x => x.SaleItem)
                    .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (sr == null) return NotFound();

            var vm = new SalesReturnViewModel
            {
                Id = sr.Id,
                ProductName = sr.SaleItem.Product.Name,
                Quantity = sr.Quantity,
                ReturnDate = sr.ReturnDate
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sr = await _context.SalesReturns.FindAsync(id);
            if (sr == null) return NotFound();

            _context.SalesReturns.Remove(sr);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper
        private async Task<List<SelectListItem>> GetProductSelectListAsync()
        {
            var saleItems = await _context.SaleItems
                .Include(x => x.Product)
                .Include(x => x.Sale)
                .ThenInclude(c => c.Client)
                .ToListAsync();

            return saleItems.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Product.Name} - {x.Sale.MemoNo} ({x.Sale.Client.Name})"
            }).ToList();
        }
    }
}
