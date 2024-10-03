namespace ChatAppMVC.DataAccessLayer.Repositories.IRepositories
{
    public interface IChatUserRepository
    {
        Task<ChatUser> GetChatUserByIdAsync(int chatUserId);
        Task CreateChatUserAsync(ChatUser chatUser);
        Task UpdateChatUserAsync(int chatUserId, ChatUser chatUser);
        Task DeleteChatUserAsync(int chatUserId);
        Task<IEnumerable<ChatUser>> GetChatUsersByChatIdAsync(int chatId);
        Task<IEnumerable<GetChatUserViewModel>> GetChatUsersByUserIdAsync(string userId);
    }
}
