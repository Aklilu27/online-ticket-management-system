using Microsoft.AspNetCore.Identity;

namespace OnlineTicketManagementSystem.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;

        public int? CompanyId { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime? LockoutUntil { get; set; }

        public bool MustChangePassword { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Company Company { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();

        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();

        public ICollection<TicketMessage> TicketMessages { get; set; } = new List<TicketMessage>();
    }
}