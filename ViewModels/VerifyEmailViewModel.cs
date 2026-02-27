using System.ComponentModel.DataAnnotations;
namespace OnlineTicketManagementSystem.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
                public string Email { get; set; } = string.Empty;
        
    }
}