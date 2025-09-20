using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class PurchaseReturnController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseReturnController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var returns = await _context.PurchaseReturns
                .Include(r => r.PurchaseItem)
                    .ThenInclude(pi => pi.Product)
                .Include(r => r.PurchaseItem.Purchase)
                    .ThenInclude(p => p.Client)
                .Select(r => new PurchaseReturnViewModel
                {
                    Id = r.Id,
                    Quantity = r.Quantity,
                    ReturnDate = r.ReturnDate,
                    ProductName = r.PurchaseItem.Product.Name,
                    SupplierName = r.PurchaseItem.Purchase.Client.Name
                })
                .ToListAsync();

            return View(returns);
        }

        public IActionResult Create()
        {
            var model = new PurchaseReturnViewModel
            {
                ReturnDate = DateTime.Today,
                PurchaseItemList = GetPurchaseItemList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseReturnViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new PurchaseReturn
                {
                    PurchaseItemId = model.PurchaseItemId,
                    Quantity = model.Quantity,
                    ReturnDate = model.ReturnDate
                };
                _context.PurchaseReturns.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.PurchaseItemList = GetPurchaseItemList();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entity = await _context.PurchaseReturns.FindAsync(id);
            if (entity == null) return NotFound();

            var model = new PurchaseReturnViewModel
            {
                Id = entity.Id,
                PurchaseItemId = entity.PurchaseItemId,
                Quantity = entity.Quantity,
                ReturnDate = entity.ReturnDate,
                PurchaseItemList = GetPurchaseItemList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseReturnViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var entity = await _context.PurchaseReturns.FindAsync(id);
                if (entity == null) return NotFound();

                entity.PurchaseItemId = model.PurchaseItemId;
                entity.Quantity = model.Quantity;
                entity.ReturnDate = model.ReturnDate;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.PurchaseItemList = GetPurchaseItemList();
            return View(model);
        }

        // GET: PurchaseReturn/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entity = await _context.PurchaseReturns
                .Include(r => r.PurchaseItem)
                    .ThenInclude(pi => pi.Product)
                .Include(r => r.PurchaseItem.Purchase)
                    .ThenInclude(p => p.Client)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null) return NotFound();

            var model = new PurchaseReturnViewModel
            {
                Id = entity.Id,
                Quantity = entity.Quantity,
                ReturnDate = entity.ReturnDate,
                ProductName = entity.PurchaseItem.Product.Name,
                SupplierName = entity.PurchaseItem.Purchase.Client.Name
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _context.PurchaseReturns.FindAsync(id);
            if (entity != null)
            {
                _context.PurchaseReturns.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper: Dropdown list generator
        private IEnumerable<SelectListItem> GetPurchaseItemList()
        {
            return _context.PurchaseItems
                .Include(pi => pi.Product)
                .Include(pi => pi.Purchase)
                    .ThenInclude(p => p.Client)
                .Select(pi => new SelectListItem
                {
                    Value = pi.Id.ToString(),
                    Text = $"{pi.Product.Name} - {pi.Purchase.MemoNo} ({pi.Purchase.Client.Name})"
                }).ToList();
        }
    }
}
