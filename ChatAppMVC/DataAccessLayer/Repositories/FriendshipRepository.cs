using AutoMapper.QueryableExtensions;
using ChatAppMVC.DataAccessLayer.Repositories.IRepositories;

namespace ChatAppMVC.DataAccessLayer.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly ChatAppDbContext _context;
        private readonly IMapper _mapper;

        public FriendshipRepository(ChatAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddFriendshipAsync(Friendship friendship)
        {
            await _context.Friendships.AddAsync(friendship);
        }

        public async Task<Friendship> GetFriendshipByIdAsync(int friendshipId)
        {
            return await _context.Friendships.FirstOrDefaultAsync(f => f.Id == friendshipId);
        }

        public async Task<FriendshipDto> GetFriendshipForTwoUsersAsync(string user1Id, string user2Id)
        {
            return await
                _context.Friendships
                .Where(f => (f.User1Id == user1Id && f.User2Id == user2Id) ||
                (f.User2Id == user1Id && f.User1Id == user2Id))
                .ProjectTo<FriendshipDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Friendship>> GetFriendshipsByUserIdAsync(string userId)
        {
            return await
                _context.Friendships
                .Where(f => f.User1Id == userId || f.User2Id == userId)
                .ToListAsync();
        }

        public async Task RemoveFriendshipAsync(int friendshipId)
        {
            var friendship = await GetFriendshipByIdAsync(friendshipId);
            if (friendship != null)
            {
                _context.Friendships.Remove(friendship);
            }
        }

        public async Task<List<GetUserViewModel>> GetUserFriends(string userId)
        {
            // get user friends
            var friendships = await GetFriendshipsByUserIdAsync(userId);
            var friends = new List<GetUserViewModel>();
            foreach (var friendship in friendships)
            {
                var friendId = friendship.User1Id == userId ? friendship.User2Id : friendship.User1Id;
                var friend = await _context.Users
                    .Where(u => u.Id == friendId)
                    .ProjectTo<GetUserViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                friends.Add(friend);
            }
            return friends;
        }
    }
}
