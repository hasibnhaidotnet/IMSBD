using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class QuotationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuotationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quotation
        public async Task<IActionResult> Index()
        {
            var quotations = await _context.Quotations
                .Include(q => q.Client)
                .Include(q => q.Items)
                .OrderByDescending(q => q.QuotationDate)
                .ToListAsync();
            return View(quotations);
        }

        // GET: Quotation/Create
        // GET: Quotation/Create
        public IActionResult Create()
        {
            var model = new QuotationViewModel
            {
                QuotationDate = DateTime.Now,
                Clients = _context.Clients
                            .Where(c => c.ClientType == ClientType.Buyer)
                            .Select(c => new SelectListItem
                            {
                                Value = c.Id.ToString(),
                                Text = c.Name
                            }).ToList(),
                Items = new List<QuotationItemViewModel>
        {
            new QuotationItemViewModel
            {
                ProductList = _context.Products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            }
        }
            };

            return View(model);
        }

        // POST: Quotation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuotationViewModel model)
        {
            if (!ModelState.IsValid || model.Items == null || !model.Items.Any())
            {
                // Reload dropdowns
                model.Clients = _context.Clients
                    .Where(c => c.ClientType == ClientType.Buyer)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();

                foreach (var item in model.Items ?? new List<QuotationItemViewModel>())
                {
                    item.ProductList = _context.Products.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToList();
                }

                return View(model);
            }

            var quotation = new Quotation
            {
                QuotationNo = "Q-" + DateTime.Now.Ticks.ToString().Substring(10),
                QuotationDate = model.QuotationDate,
                ClientId = model.ClientId,
                Items = model.Items.Select(i => new QuotationItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = (double)i.SellingPrice
                }).ToList()
            };

            _context.Quotations.Add(quotation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Quotation/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Items)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                return NotFound();

            var model = new QuotationViewModel
            {
                Id = quotation.Id,
                ClientId = quotation.ClientId,
                QuotationDate = quotation.QuotationDate,
                Clients = _context.Clients
                    .Where(c => c.ClientType == ClientType.Buyer)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList(),

                Items = quotation.Items.Select(i => new QuotationItemViewModel
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    SellingPrice = (decimal)i.Price,
                    ProductList = _context.Products.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToList()
                }).ToList()
            };

            return View(model);
        }

        // POST: Quotation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuotationViewModel model)
        {
            if (!ModelState.IsValid || model.Items == null || !model.Items.Any())
            {
                model.Clients = _context.Clients
                    .Where(c => c.ClientType == ClientType.Buyer)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();

                foreach (var item in model.Items ?? new List<QuotationItemViewModel>())
                {
                    item.ProductList = _context.Products.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToList();
                }

                return View(model);
            }

            var quotation = await _context.Quotations
                .Include(q => q.Items)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                return NotFound();

            quotation.ClientId = model.ClientId;
            quotation.QuotationDate = model.QuotationDate;

            // Remove old items
            _context.QuotationItems.RemoveRange(quotation.Items);

            // Add updated items
            quotation.Items = model.Items.Select(i => new QuotationItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = (double)i.SellingPrice
            }).ToList();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Quotation/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Client)
                .Include(q => q.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                return NotFound();

            var viewModel = new QuotationViewModel
            {
                Id = quotation.Id,
                ClientId = quotation.ClientId,
                ClientName = quotation.Client?.Name ?? "",
                QuotationDate = quotation.QuotationDate,
                Items = quotation.Items.Select(i => new QuotationItemViewModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "",
                    Quantity = i.Quantity,
                    SellingPrice = (decimal)i.Price
                }).ToList()
            };

            return View(viewModel);
        }

        // GET: Quotation/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Client)
                .Include(q => q.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                return NotFound();

            var viewModel = new QuotationViewModel
            {
                Id = quotation.Id,
                ClientId = quotation.ClientId,
                ClientName = quotation.Client?.Name ?? "",
                QuotationDate = quotation.QuotationDate,
                Items = quotation.Items.Select(i => new QuotationItemViewModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "",
                    Quantity = i.Quantity,
                    SellingPrice = (decimal)i.Price
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Quotation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Items)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                return NotFound();

            _context.QuotationItems.RemoveRange(quotation.Items);
            _context.Quotations.Remove(quotation);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Quotation/Print/5
        public async Task<IActionResult> Print(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Client)
                .Include(q => q.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                return NotFound();

            return View("Print", quotation);
        }
    }
}
