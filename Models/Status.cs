using System.Collections.Generic;

namespace OnlineTicketManagementSystem.Models
{
    public class Status
    {
        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}