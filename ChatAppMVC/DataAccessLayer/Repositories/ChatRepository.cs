using ChatAppMVC.DataAccessLayer.Repositories.IRepositories;

namespace ChatAppMVC.DataAccessLayer.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatAppDbContext _context;

        public ChatRepository(ChatAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateChatAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
        }

        public async Task DeleteChatAsync(int chatId)
        {
            var chat = await GetChatByIdAsync(chatId);

            if (chat == null) return;

            _context.Chats.Remove(chat);
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            return await _context.Chats
                .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<IEnumerable<Chat>> GetChatsByUserIdAsync(string userId)
        {
            return await _context.ChatUsers
                .Where(cu => cu.UserId == userId)
                .Select(cu => cu.Chat)
                .Distinct() 
                .ToListAsync();
        }

        public async Task<IEnumerable<Chat>> SearchChatsByName(string chatName)
        {
            return await _context.Chats
                .Where(c => c.ChatName.Contains(chatName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Chat>> SearchChatsByType(ChatType chatType)
        {
            return await _context.Chats
                .Where(c => c.Type == chatType)
                .ToListAsync();
        }

        public async Task UpdateChatAsync(int chatId, Chat chat)
        {
            var existChat = await GetChatByIdAsync(chatId);

            if (existChat == null) return;
            // change entry
            _context.Entry(existChat).CurrentValues.SetValues(chat);
        }

    }
}
