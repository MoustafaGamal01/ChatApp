namespace ChatAppMVC.DataAccessLayer.Models
{
    public enum MesaageStatus
    {
        Sent,
        Delivered,
        Read
    }
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime SentAt { get; set; }

        [ForeignKey("User")]
        public string SenderId { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public MesaageStatus Status { get; set; } = MesaageStatus.Sent;
    }
}
