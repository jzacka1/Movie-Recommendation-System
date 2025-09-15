namespace SmartReturns.Models
{
    public class ReturnRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
