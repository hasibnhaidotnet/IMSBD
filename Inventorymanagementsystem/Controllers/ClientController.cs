using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                .Select(c => new ClientViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    Phone = c.Phone,
                    Email = c.Email,
                    ClientType = c.ClientType
                }).ToListAsync();

            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = new Client
                {
                    Name = model.Name,
                    Address = model.Address,
                    Phone = model.Phone,
                    Email = model.Email,
                    ClientType = model.ClientType
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            var vm = new ClientViewModel
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Phone = client.Phone,
                Email = client.Email,
                ClientType = client.ClientType
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null) return NotFound();

                client.Name = model.Name;
                client.Address = model.Address;
                client.Phone = model.Phone;
                client.Email = model.Email;
                client.ClientType = model.ClientType;

                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound();

            // Get Purchase list & totals
            var purchases = await _context.Purchases
                .Where(p => p.ClientId == id)
                .Include(p => p.PurchaseItems)
                .ToListAsync();

            var totalPurchaseAmount = purchases.Sum(p => p.TotalAmount);
            var totalPurchasePaid = purchases.Sum(p => p.PaidAmount);
            var purchaseDue = totalPurchaseAmount - totalPurchasePaid;

            // Get Sale list & totals
            var sales = await _context.Sales
                .Where(s => s.ClientId == id)
                .Include(s => s.SaleItems)
                .ToListAsync();

            var totalSalesAmount = sales.Sum(s => s.TotalAmount);
            var totalSalesPaid = sales.Sum(s => s.PaidAmount);
            var salesDue = totalSalesAmount - totalSalesPaid;

            // Total Paid = Purchase Paid + Sale Paid
            var totalPaid = totalPurchasePaid + totalSalesPaid;

            // Total Due = Purchase Due + Sales Due
            var totalDue = purchaseDue + salesDue;

            var model = new ClientDetailsViewModel
            {
                Client = client,
                Purchases = purchases,
                Sales = sales,
                TotalPurchaseAmount = totalPurchaseAmount,
                TotalPurchasePaid = totalPurchasePaid,
                PurchaseDue = purchaseDue,
                TotalSalesAmount = totalSalesAmount,
                TotalSalesPaid = totalSalesPaid,
                SalesDue = salesDue,
                TotalPaid = totalPaid,
                TotalDue = totalDue
            };

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            var vm = new ClientViewModel
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Phone = client.Phone,
                Email = client.Email,
                ClientType = client.ClientType
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
