
namespace ChatAppMVC.Models
{
    public class ChatAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options) : base(options)
        {}

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Enable Arabic 
            builder.Entity<Chat>()
                .Property(c => c.ChatName)
                .IsUnicode(true);

            builder.Entity<ChatUser>()
                .Property(c => c.DisplayName)
                .IsUnicode(true);

            builder.Entity<Message>()
                .Property(c => c.Content)
                .IsUnicode(true);

            builder.Entity<Notification>()
                .Property(c => c.Content)
                .IsUnicode(true);
        
        
        }
    }
}
