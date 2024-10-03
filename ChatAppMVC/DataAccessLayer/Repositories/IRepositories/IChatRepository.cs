namespace ChatAppMVC.DataAccessLayer.Repositories.IRepositories
{
    public interface IChatRepository
    {
        Task<Chat> GetChatByIdAsync(int chatId);
        Task CreateChatAsync(Chat chat);
        Task UpdateChatAsync(int chatId, Chat chat);
        Task DeleteChatAsync(int chatId);
        Task<IEnumerable<Chat>> GetChatsByUserIdAsync(string userId);
        Task<IEnumerable<Chat>> SearchChatsByName(string chatName);
        Task<IEnumerable<Chat>> SearchChatsByType(ChatType chatType);
    }
}
