using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;

namespace ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatAppDbContext _context;
        public INotificationRepository NotificationRepository { get; private set; }
        public IChatRepository ChatRepository { get; private set; }
        public IChatUserRepository ChatUserRepository { get; private set; }
        public IFriendRequestRepository FriendRequestRepository { get; private set; }
        public IFriendshipRepository FriendshipRepository { get; private set; }
        public IMessageRepository MessageRepository { get; private set; }

        public UnitOfWork(ChatAppDbContext context,
            INotificationRepository notificationRepository,
            IChatRepository chatRepository,
            IChatUserRepository chatUserRepository,
            IFriendRequestRepository friendRequestRepository,
            IFriendshipRepository friendshipRepository,
            IMessageRepository messageRepository)
        {
            _context = context;
            NotificationRepository = notificationRepository;
            ChatRepository = chatRepository;
            ChatUserRepository = chatUserRepository;
            FriendRequestRepository = friendRequestRepository;
            FriendshipRepository = friendshipRepository;
            MessageRepository = messageRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
