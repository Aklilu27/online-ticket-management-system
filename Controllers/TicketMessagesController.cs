using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class TicketMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _context.TicketMessages
                .Include(t => t.Ticket)
                .Include(t => t.Sender)
                .AsNoTracking()
                .ToListAsync();

            return View(messages);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticketMessage = await _context.TicketMessages
                .Include(t => t.Ticket)
                .Include(t => t.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == id);

            if (ticketMessage == null) return NotFound();

            return View(ticketMessage);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,SenderId,Message,IsInternal,Attachment")] TicketMessage ticketMessage)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ticketMessage);
                return View(ticketMessage);
            }

            ticketMessage.SentAt = DateTime.Now;
            _context.Add(ticketMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticketMessage = await _context.TicketMessages.FindAsync(id);
            if (ticketMessage == null) return NotFound();

            await LoadLookupsAsync(ticketMessage);
            return View(ticketMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,TicketId,SenderId,Message,IsInternal,Attachment,SentAt")] TicketMessage ticketMessage)
        {
            if (id != ticketMessage.MessageId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(ticketMessage);
                return View(ticketMessage);
            }

            try
            {
                _context.Update(ticketMessage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.TicketMessages.AnyAsync(e => e.MessageId == ticketMessage.MessageId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ticketMessage = await _context.TicketMessages
                .Include(t => t.Ticket)
                .Include(t => t.Sender)
                .FirstOrDefaultAsync(m => m.MessageId == id);

            if (ticketMessage == null) return NotFound();

            return View(ticketMessage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketMessage = await _context.TicketMessages.FindAsync(id);
            if (ticketMessage != null)
            {
                _context.TicketMessages.Remove(ticketMessage);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookupsAsync(TicketMessage message = null)
        {
            ViewBag.Tickets = new SelectList(
                await _context.Tickets.OrderBy(t => t.Title).ToListAsync(),
                "TicketId", "Title", message?.TicketId);

            ViewBag.Users = new SelectList(
                await _context.Users.OrderBy(u => u.FullName).ToListAsync(),
                "Id", "FullName", message?.SenderId);
        }
    }
}
