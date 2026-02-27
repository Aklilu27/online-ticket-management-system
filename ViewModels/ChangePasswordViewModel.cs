using System.ComponentModel.DataAnnotations;
namespace OnlineTicketManagementSystem.ViewModels
{
    public class ChangePasswordViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 6 and 30 characters")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Passwords do not match")]
        public string NewPassword { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm New password")]
        public string ConfirmNewPassword { get; set; } = string.Empty;  
    }
}