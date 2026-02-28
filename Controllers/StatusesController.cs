using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class StatusesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Statuses.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var status = await _context.Statuses.FirstOrDefaultAsync(m => m.StatusId == id);
            if (status == null) return NotFound();
            return View(status);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusName")] Status status)
        {
            if (!ModelState.IsValid) return View(status);
            _context.Add(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var status = await _context.Statuses.FindAsync(id);
            if (status == null) return NotFound();
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,StatusName")] Status status)
        {
            if (id != status.StatusId) return NotFound();
            if (!ModelState.IsValid) return View(status);

            _context.Update(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var status = await _context.Statuses.FirstOrDefaultAsync(m => m.StatusId == id);
            if (status == null) return NotFound();
            return View(status);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status != null)
            {
                _context.Statuses.Remove(status);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
