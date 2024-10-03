namespace ChatAppMVC.BusinessLogicLayer.ViewModel.ChatViewModels
{
    public class GroupChatViewModel
    {
        public string CurrentUserId { get; set; }
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public string ChatPicture { get; set; }
        public ChatType ChatType { get; set; }
        public List<string> ChatUsersIds { get; set; } = new List<string>();
        public List<GetMessageDto>? Messages { get; set; }
    }
}
