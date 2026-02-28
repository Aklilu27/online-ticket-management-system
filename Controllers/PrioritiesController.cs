using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class PrioritiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrioritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Priorities.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var priority = await _context.Priorities.FirstOrDefaultAsync(m => m.PriorityId == id);
            if (priority == null) return NotFound();
            return View(priority);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriorityName")] Priority priority)
        {
            if (!ModelState.IsValid) return View(priority);
            _context.Add(priority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var priority = await _context.Priorities.FindAsync(id);
            if (priority == null) return NotFound();
            return View(priority);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriorityId,PriorityName")] Priority priority)
        {
            if (id != priority.PriorityId) return NotFound();
            if (!ModelState.IsValid) return View(priority);

            _context.Update(priority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var priority = await _context.Priorities.FirstOrDefaultAsync(m => m.PriorityId == id);
            if (priority == null) return NotFound();
            return View(priority);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);
            if (priority != null)
            {
                _context.Priorities.Remove(priority);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
