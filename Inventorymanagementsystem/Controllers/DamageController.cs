using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class DamageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DamageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Damage
        public async Task<IActionResult> Index()
        {
            var damages = await _context.Damages
                .Include(d => d.Product)
                .OrderByDescending(d => d.DamageDate)
                .Select(d => new DamageViewModel
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    DamageDate = d.DamageDate,
                    Note = d.Note
                })
                .ToListAsync();

            return View(damages);
        }

        // GET: Damage/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products.OrderBy(p => p.Name), "Id", "Name");
            return View();
        }

        // POST: Damage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DamageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var damage = new Damage
                {
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    DamageDate = model.DamageDate,
                    Note = model.Note
                };

                _context.Add(damage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(_context.Products.OrderBy(p => p.Name), "Id", "Name", model.ProductId);
            return View(model);
        }

        // GET: Damage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var damage = await _context.Damages.FindAsync(id);
            if (damage == null) return NotFound();

            var model = new DamageViewModel
            {
                Id = damage.Id,
                ProductId = damage.ProductId,
                Quantity = damage.Quantity,
                DamageDate = damage.DamageDate,
                Note = damage.Note
            };

            ViewData["ProductId"] = new SelectList(_context.Products.OrderBy(p => p.Name), "Id", "Name", damage.ProductId);
            return View(model);
        }

        // POST: Damage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DamageViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var damage = await _context.Damages.FindAsync(id);
                    if (damage == null) return NotFound();

                    damage.ProductId = model.ProductId;
                    damage.Quantity = model.Quantity;
                    damage.DamageDate = model.DamageDate;
                    damage.Note = model.Note;

                    _context.Update(damage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DamageExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(_context.Products.OrderBy(p => p.Name), "Id", "Name", model.ProductId);
            return View(model);
        }

        // GET: Damage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var damage = await _context.Damages
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (damage == null) return NotFound();

            var model = new DamageViewModel
            {
                Id = damage.Id,
                ProductId = damage.ProductId,
                ProductName = damage.Product.Name,
                Quantity = damage.Quantity,
                DamageDate = damage.DamageDate,
                Note = damage.Note
            };

            return View(model);
        }

        // GET: Damage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var damage = await _context.Damages
                .Include(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (damage == null) return NotFound();

            var model = new DamageViewModel
            {
                Id = damage.Id,
                ProductId = damage.ProductId,
                ProductName = damage.Product.Name,
                Quantity = damage.Quantity,
                DamageDate = damage.DamageDate,
                Note = damage.Note
            };

            return View(model);
        }

        // POST: Damage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var damage = await _context.Damages.FindAsync(id);
            _context.Damages.Remove(damage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DamageExists(int id)
        {
            return _context.Damages.Any(e => e.Id == id);
        }
    }
}
