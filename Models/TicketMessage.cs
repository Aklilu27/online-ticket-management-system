namespace OnlineTicketManagementSystem.Models
{
    public class TicketMessage
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int TicketId { get; set; }
    }
}