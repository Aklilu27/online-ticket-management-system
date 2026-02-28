using System;
using System.Collections.Generic;

namespace OnlineTicketManagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int CompanyId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Company Company { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}