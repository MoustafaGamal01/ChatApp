namespace ChatAppMVC.DataAccessLayer.Repositories.IRepositories
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task RemoveNotificationAsync(int notId);
        Task<Notification> GetNotificationByIdAsync(int notId);
        Task<IEnumerable<GetNotificationDto>> GetNotificationByUserIdAsync(string userId, int pageNumber);
        Task<GetNotificationDto> GetNotificationDtoByIdAsync(int notId);
        Task MarkAllNotificationsAsReadAsync(string userId);

    }
}
