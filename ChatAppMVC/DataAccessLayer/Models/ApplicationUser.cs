namespace ChatAppMVC.DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool isOnline { get; set; } = false;
        public string? ProfilePictureUrl { get; set; }
        public ICollection<Chat>? Chats { get; set; } = new List<Chat>();
	}
}
