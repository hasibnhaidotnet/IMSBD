using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubCategory
        public async Task<IActionResult> Index()
        {
            var subCategories = await _context.SubCategories
                .Include(s => s.Category)
                .ToListAsync();
            return View(subCategories);
        }

        // GET: SubCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subCategory == null) return NotFound();

            var viewModel = new SubCategoryViewModel
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
                CategoryList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = subCategory.Category.Id.ToString(),
                        Text = subCategory.Category.Name
                    }
                }
            };

            return View(viewModel);
        }
        // GET: SubCategory/Create
        public IActionResult Create()
        {
            var viewModel = new SubCategoryViewModel
            {
                CategoryList = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
            };
            return View(viewModel);
        }

        // POST: SubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var subCategory = new SubCategory
                {
                    Name = viewModel.Name,
                    CategoryId = viewModel.CategoryId
                };

                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.CategoryList = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            return View(viewModel);
        }

        // GET: SubCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory == null) return NotFound();

            var viewModel = new SubCategoryViewModel
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
                CategoryList = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
            };

            return View(viewModel);
        }

        // POST: SubCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var subCategory = await _context.SubCategories.FindAsync(id);
                    if (subCategory == null) return NotFound();

                    subCategory.Name = viewModel.Name;
                    subCategory.CategoryId = viewModel.CategoryId;

                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(viewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            viewModel.CategoryList = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            return View(viewModel);
        }

        // GET: SubCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subCategory == null) return NotFound();

            var viewModel = new SubCategoryViewModel
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
                CategoryList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = subCategory.Category.Id.ToString(),
                        Text = subCategory.Category.Name
                    }
                }
            };

            return View(viewModel);
        }

        // POST: SubCategory/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(int id)
        {
            return _context.SubCategories.Any(e => e.Id == id);
        }
    }
}
