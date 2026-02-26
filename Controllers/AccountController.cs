
using Microsoft.AspNetCore.Mvc;
namespace OnlineTicketManagementSystem.Controllers
{
    public class AccountController : Controller
    {

         public IActionResult Login()
          {
                return View();
          }  
    }
    
}