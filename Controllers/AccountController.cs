
using Microsoft.AspNetCore.Mvc;
using OnlineTicketManagementSystem.ViewModels;
using OnlineTicketManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace OnlineTicketManagementSystem.Controllers
{
    public class AccountController : Controller
    {

         public IActionResult Login()
          {
                return View();
          }  
          public IActionResult Register()
          {
                return View();
          }  

          [HttpPost]
          public async Task<IActionResult> Register(RegisterViewModel model)
          {    
            if(ModelState.IsValid){
             ApplicationUser applicationUsers = new ApplicationUser
             {
                  UserName = model.Email,
                   FullName = model.FullName,
                   Email = model.Email
             };
          }
                // Perform registration logic here (e.g., save user to database)
                // For demonstration purposes, we'll just redirect to the login page
                return RedirectToAction("Login");
          }

          public IActionResult VerifyEmail()
          {
                return View();
          }  
              public IActionResult ChangePassword()
              {
                  return View();
              }
    }
    
}