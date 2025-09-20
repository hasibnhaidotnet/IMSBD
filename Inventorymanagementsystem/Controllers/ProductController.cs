using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Unit)
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .ToListAsync();

            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            var viewModel = new ProductViewModel
            {
                UnitList = _context.Units.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }),
                CategoryList = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                SubCategoryList = _context.SubCategories.Select(sc => new SelectListItem { Value = sc.Id.ToString(), Text = sc.Name })
            };

            return View(viewModel);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.UnitList = _context.Units.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name });
                vm.CategoryList = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                vm.SubCategoryList = _context.SubCategories.Select(sc => new SelectListItem { Value = sc.Id.ToString(), Text = sc.Name });

                return View(vm);
            }

            var product = new Product
            {
                Name = vm.Name,
                Brand = vm.Brand,
                ProductCode = vm.ProductCode,
                UnitId = vm.UnitId,
                CategoryId = vm.CategoryId,
                SubCategoryId = vm.SubCategoryId,
                PurchasePrice = vm.PurchasePrice,
                SellingPrice = vm.SellingPrice,
                Notes = vm.Notes,
                MinimumStockAlert = vm.MinimumStockAlert
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                ProductCode = product.ProductCode,
                UnitId = product.UnitId,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                PurchasePrice = product.PurchasePrice,
                SellingPrice = product.SellingPrice,
                Notes = product.Notes,
                MinimumStockAlert = product.MinimumStockAlert,

                UnitList = _context.Units.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }),
                CategoryList = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }),
                SubCategoryList = _context.SubCategories.Select(sc => new SelectListItem { Value = sc.Id.ToString(), Text = sc.Name })
            };

            return View(vm);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.UnitList = _context.Units.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name });
                vm.CategoryList = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                vm.SubCategoryList = _context.SubCategories.Select(sc => new SelectListItem { Value = sc.Id.ToString(), Text = sc.Name });

                return View(vm);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = vm.Name;
            product.Brand = vm.Brand;
            product.ProductCode = vm.ProductCode;
            product.UnitId = vm.UnitId;
            product.CategoryId = vm.CategoryId;
            product.SubCategoryId = vm.SubCategoryId;
            product.PurchasePrice = vm.PurchasePrice;
            product.SellingPrice = vm.SellingPrice;
            product.Notes = vm.Notes;
            product.MinimumStockAlert = vm.MinimumStockAlert;

            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Unit)
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Product/Delete/5
        // GET
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Unit)
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
