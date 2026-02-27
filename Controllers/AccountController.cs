
using Microsoft.AspNetCore.Mvc;
using OnlineTicketManagementSystem.ViewModels;
using OnlineTicketManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace OnlineTicketManagementSystem.Controllers
{


      public class AccountController : Controller
      {
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly UserManager<ApplicationUser> _userManager;

            public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
            {
                  _signInManager = signInManager;
                  _userManager = userManager;
            }
            public IActionResult Login()
            {
                  return View();
            }
            [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel model)
            {
                  if (ModelState.IsValid)
                  {
                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                              return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                              ModelState.AddModelError("", "Invalid login attempt. weather email or password is incorrect.");
                              return View(model);
                        }
                  }
                  return View(model);
            }
            public IActionResult Register()
            {
                  return View();
            }

            [HttpPost]
            public async Task<IActionResult> Register(RegisterViewModel model)
            {
                  if (ModelState.IsValid)
                  {
                        ApplicationUser applicationUsers = new ApplicationUser
                        {

                              FullName = model.FullName,
                              Email = model.Email,
                              UserName = model.Email
                        };
                        var result = await _userManager.CreateAsync(applicationUsers, model.Password);
                        // Perform registration logic here (e.g., save user to database)
                        // For demonstration purposes, we'll just redirect to the login page
                        if (result.Succeeded)
                        {
                              return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                              foreach (var error in result.Errors)
                              {
                                    ModelState.AddModelError("", error.Description);
                              }
                              // If we got this far, something failed; redisplay form
                              return View(model);
                        }

                  }
                  // If ModelState is not valid, redisplay the form with validation errors
                  return View(model);
            }



            public IActionResult VerifyEmail()
            {
                  return View();
            }
            [HttpPost]
            public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
            {
                  if (ModelState.IsValid)
                  {
                        var user = await _userManager.FindByEmailAsync(model.Email);
                        if (user == null)
                        {
                              ModelState.AddModelError("", "No user found with the provided email.");
                              return View(model);
                        }
                        else
                        {
                              // Perform verification logic here
                              return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });

                        }

                  }

                  return View(model);
            }
            public IActionResult ChangePassword(string username)

            {
                  if (string.IsNullOrEmpty(username))
                  {
                        return RedirectToAction("VerifyEmail", "Account");
                  }
                  return View(new ChangePasswordViewModel { Email = username });
            }

            [HttpPost]
            public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
            {
                  if (ModelState.IsValid)
                  {
                        var user = await _userManager.FindByNameAsync(model.Email);
                        if (user != null)
                        {
                              var result = await _userManager.RemovePasswordAsync(user);
                              if (result.Succeeded)
                              {
                                    result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                                    return RedirectToAction("Login", "Account");
                              }
                              else
                              {
                                    foreach (var error in result.Errors)
                                    {
                                          ModelState.AddModelError("", error.Description);
                                    }
                                    return View(model);
                              }
                        }
                        else
                        {
                              ModelState.AddModelError("", "email is not found");
                              return View(model);
                        }
                  }
                  else
                  {
                        ModelState.AddModelError("", "Something went wrong.");
                        return View(model);
                  }
            }
            
             [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

      }
}


