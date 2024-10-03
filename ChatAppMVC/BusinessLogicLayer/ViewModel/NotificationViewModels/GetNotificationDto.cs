namespace ChatAppMVC.BusinessLogicLayer.Dtos.NotificatioViewModels
{
    public class GetNotificationDto
    {
        public bool isRead { get; set; }
        public string Content { get; set; }
        public string SenderName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
