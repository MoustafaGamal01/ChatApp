namespace ChatAppMVC.DataAccessLayer.Repositories.IRepositories
{
    public interface IFriendshipRepository
    {
        Task AddFriendshipAsync(Friendship friendship);
        Task RemoveFriendshipAsync(int friendshipId);
        Task<IEnumerable<Friendship>> GetFriendshipsByUserIdAsync(string userId);
        Task<Friendship> GetFriendshipByIdAsync(int friendshipId);
        Task<FriendshipDto> GetFriendshipForTwoUsersAsync(string user1Id, string user2Id);
        Task<List<GetUserViewModel>> GetUserFriends(string userId);
    }
}
