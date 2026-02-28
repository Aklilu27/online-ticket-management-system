using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;
using OnlineTicketManagementSystem.ViewModels;

namespace OnlineTicketManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        // =============================
        // LOGIN PAGE
        // =============================

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Account is disabled");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                true
            );

            if (result.Succeeded)
            {
                user.LastLoginAt = DateTime.Now;
                user.FailedLoginAttempts = 0;
                user.LockoutUntil = null;
                await _userManager.UpdateAsync(user);

                // Force password change (Documentation rule)
                if (user.MustChangePassword)
                {
                    return RedirectToAction("ChangePassword", new { email = user.Email });
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is temporarily locked. Please try again later.");
                return View(model);
            }

            user.FailedLoginAttempts += 1;
            if (user.FailedLoginAttempts >= 5)
            {
                user.LockoutUntil = DateTime.Now.AddMinutes(15);
            }

            await _userManager.UpdateAsync(user);

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        // =============================
        // REGISTER USER (ADMIN ONLY)
        // =============================

        public async Task<IActionResult> Register()
        {
            await LoadRegisterLookupsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadRegisterLookupsAsync();
                return View(model);
            }

            var selectedRole = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Id == model.RoleId);

            if (selectedRole == null)
            {
                ModelState.AddModelError(nameof(model.RoleId), "Selected role is invalid.");
                await LoadRegisterLookupsAsync();
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                CompanyId = model.CompanyId,
                EmailConfirmed = true,
                LockoutEnabled = true,
                FailedLoginAttempts = 0,
                MustChangePassword = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, selectedRole.Name!);
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            await LoadRegisterLookupsAsync();

            return View(model);
        }

        // =============================
        // VERIFY EMAIL
        // =============================

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email not found");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Account is disabled");
                return View(model);
            }

            return RedirectToAction("ChangePassword", new { email = user.Email });
        }

        // =============================
        // CHANGE PASSWORD
        // =============================

        public IActionResult ChangePassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            return View(new ChangePasswordViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                user.MustChangePassword = false;
                user.FailedLoginAttempts = 0;
                user.LockoutUntil = null;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // =============================
        // LOGOUT
        // =============================

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task LoadRegisterLookupsAsync()
        {
            var companies = await _dbContext.Companies
                .OrderBy(c => c.Name)
                .ToListAsync();

            var roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();

            ViewBag.Companies = new SelectList(companies, "Id", "Name");
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
        }
    }
}