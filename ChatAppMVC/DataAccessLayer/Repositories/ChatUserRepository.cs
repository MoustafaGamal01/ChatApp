using AutoMapper.QueryableExtensions;
using ChatAppMVC.DataAccessLayer.Repositories.IRepositories;

namespace ChatAppMVC.DataAccessLayer.Repositories
{
    public class ChatUserRepository : IChatUserRepository
    {
        private readonly ChatAppDbContext _context;
        private readonly IMapper _mapper;

        public ChatUserRepository(ChatAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ChatUser> GetChatUserByIdAsync(int chatUserId)
        {
            return await _context.ChatUsers.FirstOrDefaultAsync(c => c.Id == chatUserId);
        }

        public async Task CreateChatUserAsync(ChatUser chatUser)
        {
            await _context.ChatUsers.AddAsync(chatUser);
        }

        public async Task UpdateChatUserAsync(int chatUserId, ChatUser chatUser)
        {
            var exitingChatUser = await _context.ChatUsers.FindAsync(chatUserId);

            if (exitingChatUser == null) return;

            if (!string.IsNullOrEmpty(exitingChatUser.DisplayName) && exitingChatUser.DisplayName != chatUser.DisplayName)
                exitingChatUser.DisplayName = chatUser.DisplayName;

            if (exitingChatUser.LastReadMessageId != chatUser.LastReadMessageId)
                exitingChatUser.LastReadMessageId = chatUser.LastReadMessageId;

            _context.ChatUsers.Update(exitingChatUser);
        }

        public async Task DeleteChatUserAsync(int chatUserId)
        {
            var chatUser = await _context.ChatUsers.FindAsync(chatUserId);
            if (chatUser == null) return;

            _context.ChatUsers.Remove(chatUser);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatUser>> GetChatUsersByChatIdAsync(int chatId)
        {
            return await _context.ChatUsers
                .Include(cu => cu.User)
                .Include(cu => cu.Chat)
                .Where(cu => cu.ChatId == chatId).ToListAsync();
        }

        public async Task<IEnumerable<GetChatUserViewModel>> GetChatUsersByUserIdAsync(string userId)
        {
            return await
                _context.ChatUsers
                .Include(cu => cu.User)
                .Include(cu => cu.Chat)
                .Where(cu => cu.UserId == userId)
                .ProjectTo<GetChatUserViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(o=>o.LastMessageTime)
                .ToListAsync();
        }
    }
}
