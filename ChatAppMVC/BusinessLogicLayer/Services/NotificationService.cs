using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;
using Microsoft.Extensions.Logging;

namespace ChatAppMVC.BusinessLogicLayer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager,
            ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string receiverId, string senderId, string content)
        {
            var sender = await _userManager.FindByIdAsync(senderId);
                // Create a new notification
            var notification = new Notification
            {
                ReceiverId = receiverId,
                SenderId = senderId,
                Content = $"{sender.UserName}" + content,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
            };
            // Store notification in the database
            await _unitOfWork.NotificationRepository.AddNotificationAsync(notification);
            await _unitOfWork.CompleteAsync();

            await _hubContext.Clients.User(receiverId).SendAsync("ReceiveNotification", _mapper.Map<GetNotificationDto>(notification));
        }

        public async Task<List<GetNotificationDto>> GetUserNotificationsAsync(string userId, int pageNumber)
        {
            try
            {
                var notifications = await _unitOfWork.NotificationRepository.GetNotificationByUserIdAsync(userId, pageNumber);
                return _mapper.Map<List<GetNotificationDto>>(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications for user {UserId}", userId);
                throw; 
            }
        }

        public async Task MarkAllNotificationsAsReadAsync(string userId)
        {
            await _unitOfWork.NotificationRepository.MarkAllNotificationsAsReadAsync(userId);
            await _unitOfWork.CompleteAsync();
        }
    }
}

