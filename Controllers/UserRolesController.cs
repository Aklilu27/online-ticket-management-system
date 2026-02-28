using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mappings = await _context.UserRoles
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();

            return View(mappings);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] UserRole userRole)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(userRole.UserId, userRole.RoleId);
                return View(userRole);
            }

            var exists = await _context.UserRoles.AnyAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId);
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "This user-role mapping already exists.");
                await LoadLookupsAsync(userRole.UserId, userRole.RoleId);
                return View(userRole);
            }

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? userId, int? roleId)
        {
            if (userId == null || roleId == null) return NotFound();

            var mapping = await _context.UserRoles
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (mapping == null) return NotFound();

            return View(mapping);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int roleId)
        {
            var mapping = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (mapping != null)
            {
                _context.UserRoles.Remove(mapping);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookupsAsync(object selectedUser = null, object selectedRole = null)
        {
            ViewBag.Users = new SelectList(
                await _context.Users.OrderBy(u => u.FullName).ToListAsync(),
                "Id", "FullName", selectedUser);

            ViewBag.Roles = new SelectList(
                await _context.Roles.OrderBy(r => r.Name).ToListAsync(),
                "Id", "Name", selectedRole);
        }
    }
}
