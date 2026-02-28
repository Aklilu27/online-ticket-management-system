using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UsersController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.Company)
                .AsNoTracking()
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            ViewBag.Roles = await _userManager.GetRolesAsync(user);
            return View(user);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupsAsync();
            return View(new UserCreateInput());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(input.CompanyId, input.RoleId);
                return View(input);
            }

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == input.RoleId);
            if (role == null)
            {
                ModelState.AddModelError(nameof(input.RoleId), "Invalid role selected.");
                await LoadLookupsAsync(input.CompanyId, input.RoleId);
                return View(input);
            }

            var user = new User
            {
                FullName = input.FullName,
                Email = input.Email,
                UserName = input.Email,
                CompanyId = input.CompanyId,
                IsActive = input.IsActive,
                MustChangePassword = true,
                EmailConfirmed = true,
                LockoutEnabled = true,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, input.TemporaryPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role.Name!);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            await LoadLookupsAsync(input.CompanyId, input.RoleId);
            return View(input);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var selectedRole = await _roleManager.Roles
                .FirstOrDefaultAsync(r => roles.Contains(r.Name!));

            var input = new UserEditInput
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                CompanyId = user.CompanyId,
                RoleId = selectedRole?.Id,
                IsActive = user.IsActive,
                MustChangePassword = user.MustChangePassword
            };

            await LoadLookupsAsync(input.CompanyId, input.RoleId);
            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditInput input)
        {
            if (id != input.Id) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync(input.CompanyId, input.RoleId);
                return View(input);
            }

            user.FullName = input.FullName;
            user.Email = input.Email;
            user.UserName = input.Email;
            user.CompanyId = input.CompanyId;
            user.IsActive = input.IsActive;
            user.MustChangePassword = input.MustChangePassword;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                await LoadLookupsAsync(input.CompanyId, input.RoleId);
                return View(input);
            }

            if (input.RoleId.HasValue)
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == input.RoleId.Value);
                if (role != null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (currentRoles.Any())
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    await _userManager.AddToRoleAsync(user, role.Name!);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookupsAsync(object selectedCompany = null, object selectedRole = null)
        {
            var companies = await _context.Companies.OrderBy(c => c.Name).ToListAsync();
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();

            ViewBag.Companies = new SelectList(companies, "Id", "Name", selectedCompany);
            ViewBag.Roles = new SelectList(roles, "Id", "Name", selectedRole);
        }

        public class UserCreateInput
        {
            [Required]
            [StringLength(100)]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public int CompanyId { get; set; }

            [Required]
            public int RoleId { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [MinLength(8)]
            public string TemporaryPassword { get; set; } = string.Empty;

            public bool IsActive { get; set; } = true;
        }

        public class UserEditInput
        {
            [Required]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public int? CompanyId { get; set; }

            public int? RoleId { get; set; }

            public bool IsActive { get; set; }

            public bool MustChangePassword { get; set; }
        }
    }
}
