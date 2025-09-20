using Inventorymanagementsystem.Data;
using Inventorymanagementsystem.Models;
using Inventorymanagementsystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagementsystem.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public IActionResult Index()
        {
            var transactions = _context.Transactions.Include(t => t.Client).ToList();

            var viewModelList = transactions.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                ClientId = t.ClientId,
                ClientName = t.Client?.Name ?? "",
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                Note = t.Note
            }).ToList();

            return View(viewModelList);
        }

        // GET: Transactions/Details/5
        public IActionResult Details(int id)
        {
            var transaction = _context.Transactions
                .Include(t => t.Client)
                .FirstOrDefault(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            var viewModel = new TransactionViewModel
            {
                Id = transaction.Id,
                ClientId = transaction.ClientId,
                ClientName = transaction.Client?.Name ?? "",
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Note = transaction.Note
            };

            return View(viewModel); 
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            var viewModel = new TransactionViewModel
            {
                Clients = _context.Clients
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList(),
                TransactionDate = DateTime.Today
            };

            return View(viewModel);
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Clients = _context.Clients
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();

                return View(viewModel);
            }

            var transaction = new Transaction
            {
                ClientId = viewModel.ClientId,
                TransactionType = viewModel.TransactionType,
                Amount = viewModel.Amount,
                TransactionDate = viewModel.TransactionDate,
                Note = viewModel.Note
            };

            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound();

            var viewModel = new TransactionViewModel
            {
                Id = transaction.Id,
                ClientId = transaction.ClientId,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Note = transaction.Note,
                Clients = _context.Clients
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
            };

            return View(viewModel);
        }

        // POST: Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransactionViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                viewModel.Clients = _context.Clients
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();

                return View(viewModel);
            }

            try
            {
                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null) return NotFound();

                transaction.ClientId = viewModel.ClientId;
                transaction.TransactionType = viewModel.TransactionType;
                transaction.Amount = viewModel.Amount;
                transaction.TransactionDate = viewModel.TransactionDate;
                transaction.Note = viewModel.Note;

                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transactions.Any(e => e.Id == viewModel.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Transactions/Delete/5
        // GET: Transactions/Delete/5
        public IActionResult Delete(int id)
        {
            var transaction = _context.Transactions
                .Include(t => t.Client)
                .FirstOrDefault(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            var viewModel = new TransactionViewModel
            {
                Id = transaction.Id,
                ClientId = transaction.ClientId,
                ClientName = transaction.Client?.Name,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Note = transaction.Note
            };

            return View(viewModel);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
                return NotFound();

            _context.Transactions.Remove(transaction);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
