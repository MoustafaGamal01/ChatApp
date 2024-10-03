namespace ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork
{
    public interface IUnitOfWork
    {
        INotificationRepository NotificationRepository { get; }
        IChatRepository ChatRepository { get; }
        IChatUserRepository ChatUserRepository { get; }
        IFriendRequestRepository FriendRequestRepository { get; }
        IFriendshipRepository FriendshipRepository { get; }
        IMessageRepository MessageRepository { get; }
        Task<int> CompleteAsync();
    }
}
