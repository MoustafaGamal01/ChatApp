public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IChatService _chatService;
    public ChatHub(IMessageService messageService, IChatService chatService)
    {
        _chatService = chatService;
        _messageService = messageService;
    }
    
    public async Task SendMessage(int chatId, string senderId, string content, IFormFile file)
    {
        await _messageService.SendMessageAsync(chatId, senderId, content, file);
    }

    public async Task JoinChat(int chatId)
    {
        string groupName = chatId.ToString(); 
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await _chatService.JoinChatAsync(chatId, Context.ConnectionId);
    }

    public async Task LeaveChat(int chatId)
    {
        string groupName = chatId.ToString(); 
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await _chatService.LeaveChatAsync(chatId, Context.ConnectionId);
    }

}