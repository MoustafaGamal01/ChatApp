namespace ChatAppMVC.DataAccessLayer.Models
{
    // we don't need it for now 
    public enum NotificationType
    {
        FriendRequest,
        Message
    }
    public class Notification
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }

        [ForeignKey("Sender")]
        public string? SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}
