namespace OnlineTicketManagementSystem.Models
{
  public class AuditLog
    {
        public int LogId { get; set; }

        public int UserId { get; set; }

        public string Action { get; set; }

        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string IPAddress { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}