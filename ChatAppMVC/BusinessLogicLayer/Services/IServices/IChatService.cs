namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public interface IChatService
    {
        Task<List<GetChatUserViewModel>> LoadChatsAsync(string userId);
        Task JoinChatAsync(int chatId, string connectionId);
        Task LeaveChatAsync(int chatId, string connectionId);
        Task <Chat> GetChatByIdAsync(int chatId);
        Task<GroupChatViewModel> CreateGroupChatAsync(string groupName, IFormFile file, List<string> userIds, string adminId);
        Task<ChatViewModel> OpenChatAsync(string userId, int chatId);
        Task<GroupChatViewModel> OpenGroupChatAsync(string currentUserId, int chatId);
    }
}
