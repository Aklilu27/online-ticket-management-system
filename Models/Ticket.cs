namespace OnlineTicketManagementSystem.Models
{
   public class Ticket
    {
        public int TicketId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int StatusId { get; set; }

        public int PriorityId { get; set; }

        public int CompanyId { get; set; }

        public int ProductId { get; set; }

        public int CreatedBy { get; set; }

        public int? AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public Company Company { get; set; }

        public Product Product { get; set; }

        public User Creator { get; set; }

        public User Assignee { get; set; }

        public ICollection<TicketMessage> Messages { get; set; }
    }
}