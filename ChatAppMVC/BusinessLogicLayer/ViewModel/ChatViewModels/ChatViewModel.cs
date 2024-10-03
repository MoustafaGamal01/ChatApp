namespace ChatAppMVC.BusinessLogicLayer.ViewModel.ChatViewModels
{
    public class ChatViewModel
    {
        public int ChatId { get; set; }
        public string CurrentUserId { get; set; }
        public UserInfoViewModel CurrentUser { get; set; }
        public UserInfoViewModel OtherUser { get; set; }
        public List<GetMessageDto> Messages { get; set; }
    }
}
