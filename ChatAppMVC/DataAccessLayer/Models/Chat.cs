namespace ChatAppMVC.DataAccessLayer.Models
{
    public enum ChatType
    {
        Private,
        Group
    }
    public class Chat
    {
        public int Id { get; set; }
        public string? ChatName { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<ChatUser> ChatUsers { get; set; }
        public string? ChatPictureUrl { get; set; }
        public ChatType Type { get; set; }
    }
}
