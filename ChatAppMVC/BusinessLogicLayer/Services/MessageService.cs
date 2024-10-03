using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;

namespace ChatAppMVC.BusinessLogicLayer.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageService(IUnitOfWork unitOfWorkRepo, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWorkRepo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(int chatId, string senderId, string content, IFormFile file)
        {
            string filePath = null;
            if (file != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                filePath = Path.Combine(uploads, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            var message = new Message
            {
                ChatId = chatId,
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.Now,
                Status = MesaageStatus.Sent,
                MediaUrl = file != null ? $"/images/{file.FileName}" : null
            };
            // Add message to the database
            await _unitOfWork.MessageRepository.AddMessageAsync(message);
            await _unitOfWork.CompleteAsync();

            var chat = await _unitOfWork.ChatRepository.GetChatByIdAsync(chatId);
           
            await _hubContext.Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", _mapper.Map<GetMessageDto>(message));
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId)
        {
            return await _unitOfWork.MessageRepository.GetMessagesByChatIdAsync(chatId);
        }
    }
}
