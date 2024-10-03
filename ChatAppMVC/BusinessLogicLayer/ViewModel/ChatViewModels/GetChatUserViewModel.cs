namespace ChatAppMVC.BusinessLogicLayer.ViewModel.ChatViewModels
{
    public class GetChatUserViewModel
    {
        public int ChatId { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageTime { get; set; }
        public string ChatName { get; set; }
        public string ChatPictureUrl { get; set; }
        public ChatType ChatType { get; set; }
    }
}
