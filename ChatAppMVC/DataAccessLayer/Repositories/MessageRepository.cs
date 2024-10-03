using AutoMapper.QueryableExtensions;
using ChatAppMVC.DataAccessLayer.Models;

namespace ChatAppMVC.BusinessLogicLayer.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatAppDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(ChatAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        int pageSize = 100;
        
        public async Task<IEnumerable<GetMessageDto>> GetMessagesByChatIdPagedAsync(int chatId, int pageNumber)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.SentAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<GetMessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _context.Messages
                .Include(m => m.Chat)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<GetMessageDto>> GetMessagesBySenderIdAsync(string senderId, int pageNumber)
        {
            return await _context.Messages
                .Include(m => m.User)
                .Where(m => m.SenderId == senderId)
                .OrderBy(m => m.SentAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<GetMessageDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        }

        // Retrieve messages in a chat after a specific date
        //public async Task<IEnumerable<Message>> GetMessagesInChatAfterDateAsync(int chatId, DateTime afterDate)
        //{
        //    return await _context.Messages
        //        .Where(m => m.ChatId == chatId && m.SentAt > afterDate)
        //        .OrderBy(m => m.SentAt)
        //        .ToListAsync();
        //}

        public async Task AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task DeleteMessageAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
            }
        }

        public async Task UpdateMessageAsync(int messageId, Message message)
        {
            var existMessage = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if (existMessage != null)
            {
                existMessage.Content = message.Content;
                _context.Messages.Update(existMessage);
            }
        }
    }
}
