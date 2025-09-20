using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Unit
        public async Task<IActionResult> Index()
        {
            var units = await _context.Units
                .Select(u => new UnitViewModel
                {
                    Id = u.Id,
                    Name = u.Name
                }).ToListAsync();

            return View(units);
        }

        // GET: Unit/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();

            var viewModel = new UnitViewModel
            {
                Id = unit.Id,
                Name = unit.Name
            };

            return View(viewModel);
        }

        // GET: Unit/Create
        public IActionResult Create()
        {
            return View(new UnitViewModel());
        }

        // POST: Unit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnitViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var unit = new Unit
                {
                    Name = viewModel.Name
                };
                _context.Units.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Unit/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();

            var viewModel = new UnitViewModel
            {
                Id = unit.Id,
                Name = unit.Name
            };

            return View(viewModel);
        }

        // POST: Unit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UnitViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var unit = await _context.Units.FindAsync(id);
                if (unit == null) return NotFound();

                unit.Name = viewModel.Name;
                _context.Update(unit);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Unit/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();

            var viewModel = new UnitViewModel
            {
                Id = unit.Id,
                Name = unit.Name
            };

            return View(viewModel);
        }

        // POST: Unit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            if (unit == null) return NotFound();

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
