namespace ChatAppMVC.DataAccessLayer.Models.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;

        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task SendNotification(string receiverId, string content, string senderId)
        {
            await _notificationService.SendNotificationAsync(receiverId, content, senderId);
        }
    }
}
