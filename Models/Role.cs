using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketManagementSystem.Models
{
    public class Role : IdentityRole<int>
    {
        [NotMapped]
        public string RoleName
        {
            get => Name ?? string.Empty;
            set => Name = value;
        }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}