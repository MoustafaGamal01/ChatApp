namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public interface IChatUserService
    {
        Task<IEnumerable<GetChatUserViewModel>> GetChatUsersByUserId(string userId);
        Task<ChatUser> GetChatUserByIdAsync(int chatUserId);

    }
}
