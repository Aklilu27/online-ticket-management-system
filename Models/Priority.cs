using System.Collections.Generic;
namespace OnlineTicketManagementSystem.Models
{
    public class Priority
    {
        public int PriorityId { get; set; }

        public string PriorityName { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}