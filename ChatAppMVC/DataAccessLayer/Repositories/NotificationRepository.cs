using AutoMapper.QueryableExtensions;

namespace ChatAppMVC.DataAccessLayer.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ChatAppDbContext _context;
        private readonly IMapper _mapper;

        public NotificationRepository(ChatAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        // in case we need it
        public async Task<GetNotificationDto> GetNotificationDtoByIdAsync(int notId)
        {
            return await _context.Notifications
                .Where(n => n.Id == notId)
                .Select(n => new GetNotificationDto
                {
                    Content = n.Content,
                    SenderName = n.Sender.UserName,
                    CreatedAt = n.CreatedAt
                })
                .ProjectTo<GetNotificationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int notId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notId);
        }

        int pageSize = 10;

        public async Task<IEnumerable<GetNotificationDto>> GetNotificationByUserIdAsync(string userId, int pageNumber)
        {
            return await _context.Notifications
                .Where(n => n.ReceiverId == userId)
                .OrderBy(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<GetNotificationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task RemoveNotificationAsync(int notId)
        {
            var not = await GetNotificationByIdAsync(notId);
            if (not != null)
            {
                _context.Notifications.Remove(not);
            }
        }

        public async Task MarkAllNotificationsAsReadAsync(string userId)
        {
            var nots = await GetNotificationByUserIdAsync(userId, 1);

            foreach (var item in nots)
            {
                item.isRead = true;
            }
        }
    }
}
