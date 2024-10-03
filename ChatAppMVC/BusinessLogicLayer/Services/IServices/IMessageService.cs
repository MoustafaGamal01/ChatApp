namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public interface IMessageService
    {
        Task SendMessageAsync(int chatId, string senderId, string content, IFormFile file);
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId);
    }
}
