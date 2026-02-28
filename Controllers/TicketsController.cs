using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.Company)
                .Include(t => t.Product)
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .AsNoTracking()
                .ToListAsync();

            return View(tickets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.Company)
                .Include(t => t.Product)
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .Include(t => t.Messages)
                .FirstOrDefaultAsync(m => m.TicketId == id);

            if (ticket == null) return NotFound();

            return View(ticket);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,StatusId,PriorityId,CompanyId,ProductId,CreatedBy,AssignedTo")] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ticket);
                return View(ticket);
            }

            ticket.CreatedAt = DateTime.Now;
            _context.Add(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            await LoadLookupsAsync(ticket);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,Title,Description,StatusId,PriorityId,CompanyId,ProductId,CreatedBy,AssignedTo,CreatedAt,UpdatedAt,ResolvedAt,ClosedAt")] Ticket ticket)
        {
            if (id != ticket.TicketId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ticket);
                return View(ticket);
            }

            ticket.UpdatedAt = DateTime.Now;

            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tickets.AnyAsync(e => e.TicketId == ticket.TicketId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.Company)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.TicketId == id);

            if (ticket == null) return NotFound();

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookupsAsync(Ticket ticket = null)
        {
            ViewBag.Statuses = new SelectList(
                await _context.Statuses.OrderBy(s => s.StatusName).ToListAsync(),
                "StatusId", "StatusName", ticket?.StatusId);

            ViewBag.Priorities = new SelectList(
                await _context.Priorities.OrderBy(p => p.PriorityName).ToListAsync(),
                "PriorityId", "PriorityName", ticket?.PriorityId);

            ViewBag.Companies = new SelectList(
                await _context.Companies.OrderBy(c => c.Name).ToListAsync(),
                "Id", "Name", ticket?.CompanyId);

            ViewBag.Products = new SelectList(
                await _context.Products.OrderBy(p => p.ProductName).ToListAsync(),
                "ProductId", "ProductName", ticket?.ProductId);

            ViewBag.Users = new SelectList(
                await _context.Users.OrderBy(u => u.FullName).ToListAsync(),
                "Id", "FullName", ticket?.CreatedBy);

            ViewBag.Assignees = new SelectList(
                await _context.Users.OrderBy(u => u.FullName).ToListAsync(),
                "Id", "FullName", ticket?.AssignedTo);
        }
    }
}
