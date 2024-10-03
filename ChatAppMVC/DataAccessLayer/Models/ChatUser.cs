namespace ChatAppMVC.DataAccessLayer.Models
{
    public enum ChatUserRole
    {
        Admin,
        User
    }
    public class ChatUser
    {
        public int Id { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public ChatUserRole Role { get; set; }
        public string? ChatPictureUrl { get; set; }

        [ForeignKey("Message")]
        public int? LastReadMessageId { get; set; }
        public Message Message { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("chat")]
        public int? ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
