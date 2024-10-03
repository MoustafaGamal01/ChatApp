namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string receiverId, string senderId, string content);
        Task<List<GetNotificationDto>> GetUserNotificationsAsync(string userId, int pageNumber);
        Task MarkAllNotificationsAsReadAsync(string userId);
    }

}
