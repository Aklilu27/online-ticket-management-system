using System.ComponentModel.DataAnnotations;

namespace OnlineTicketManagementSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company is required")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, MinimumLength = 8,
        ErrorMessage = "Password must be between 8 and 30 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}