using AutoMapper.QueryableExtensions;

namespace ChatAppMVC.DataAccessLayer.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly ChatAppDbContext _context;
        private readonly IMapper _mapper;
        public FriendRequestRepository(ChatAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task <RequestStatus?> GetFriendRequestStatusAsync(string senderId, string receiverId)
        {
            FriendRequest? friendRequest = await _context.FriendRequests
                .Where(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId)
                .FirstOrDefaultAsync();
            return friendRequest?.Status ?? null;

        }
        public async Task<bool> AcceptFriendRequestAsync(int id)
        {
            var friendRequest = await GetFriendRequestByIdAsync(id);
            if (friendRequest == null)
            {
                return false;
            }
            friendRequest.Status = RequestStatus.Accepted;
            _context.FriendRequests.Update(friendRequest);
            return true;
        }

        public async Task AddFriendRequestAsync(FriendRequest friendRequest)
        {
            await _context.FriendRequests.AddAsync(friendRequest);
        }

        public async Task CancelFriendRequestAsync(int id)
        {
            var friendRequest = await GetFriendRequestByIdAsync(id);
            if (friendRequest != null)
            {
                _context.FriendRequests.Remove(friendRequest);
            }
        }

        public async Task<FriendRequest> GetFriendRequestByIdAsync(int id)
        {
            return  await _context.FriendRequests
                .Include(fr => fr.Sender)
                .Include(fr => fr.Receiver)
                .FirstOrDefaultAsync(fr => fr.Id == id);
        }

        public async Task<IEnumerable<GetFriendRequestDto>> GetFriendRequestsAsync(string userId)
        {
            return await _context.FriendRequests
                .Include(fr => fr.Sender)
                .Include(fr => fr.Receiver)
                .Where(fr => fr.ReceiverId == userId && fr.Status == RequestStatus.Pending)
                .ProjectTo<GetFriendRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<FriendRequest> GetPendingRequestAsync(string senderId, string receiverId)
        {
            return await _context.FriendRequests
                .Include(fr => fr.Sender)
                .Include(fr => fr.Receiver)
                .Where(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId && fr.Status == RequestStatus.Pending)
                .FirstOrDefaultAsync();
        }
    }
}
