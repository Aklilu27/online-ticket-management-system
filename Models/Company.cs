using System;
using System.Collections.Generic;

namespace OnlineTicketManagementSystem.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<User> Users { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}