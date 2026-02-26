using System.ComponentModel.DataAnnotations;
namespace OnlineTicketManagementSystem.ViewModels
{
    public class RegisterViewModel
    {  [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 6 and 30 characters")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords do not match")]
         [Display(Name ="New password")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm New password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public int CompanyId { get; set; }
    }
}