using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null) return NotFound();

            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleName")] Role role)
        {
            if (!ModelState.IsValid) return View(role);

            _context.Add(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoleName") ] Role role)
        {
            if (id != role.Id) return NotFound();
            if (!ModelState.IsValid) return View(role);

            var existing = await _context.Roles.FindAsync(id);
            if (existing == null) return NotFound();

            existing.RoleName = role.RoleName;
            _context.Update(existing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
