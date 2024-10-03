namespace ChatAppMVC.BusinessLogicLayer.ViewModel.MessageViewModels
{
    public class GetMessageDto
    {
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string MediaUrl { get; set; }
    }
}
