using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class AuditLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _context.AuditLogs
                .OrderByDescending(a => a.TimeStamp)
                .AsNoTracking()
                .ToListAsync();

            return View(logs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var auditLog = await _context.AuditLogs.FirstOrDefaultAsync(m => m.LogId == id);
            if (auditLog == null) return NotFound();

            return View(auditLog);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Action,EntityType,EntityId,OldValue,NewValue,IPAddress")] AuditLog auditLog)
        {
            if (!ModelState.IsValid) return View(auditLog);

            auditLog.TimeStamp = DateTime.Now;
            _context.Add(auditLog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var auditLog = await _context.AuditLogs.FirstOrDefaultAsync(m => m.LogId == id);
            if (auditLog == null) return NotFound();

            return View(auditLog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auditLog = await _context.AuditLogs.FindAsync(id);
            if (auditLog != null)
            {
                _context.AuditLogs.Remove(auditLog);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
