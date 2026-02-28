namespace OnlineTicketManagementSystem.Models
{
      public class TicketMessage
    {
        public int MessageId { get; set; }

        public int TicketId { get; set; }

        public int SenderId { get; set; }

        public string Message { get; set; }

        public bool IsInternal { get; set; }

        public string Attachment { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public Ticket Ticket { get; set; }

        public User Sender { get; set; }
    }
}