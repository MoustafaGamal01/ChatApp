namespace ChatAppMVC.DataAccessLayer.Repositories.IRepositories
{
    public interface IMessageRepository
    {
        Task<Message> GetMessageByIdAsync(int id);
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId);
        Task<IEnumerable<GetMessageDto>> GetMessagesBySenderIdAsync(string senderId, int pageNumber);
        Task<IEnumerable<GetMessageDto>> GetMessagesByChatIdPagedAsync(int chatId, int pageNumber);
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(int id);
        Task UpdateMessageAsync(int messageId, Message message);
    }
}
