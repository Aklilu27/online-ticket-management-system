using Microsoft.AspNetCore.Identity;

namespace OnlineTicketManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public bool MustChangePassword { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}