namespace ChatAppMVC.DataAccessLayer.Repositories.IRepositories
{
    public interface IFriendRequestRepository
    {
        Task<IEnumerable<GetFriendRequestDto>> GetFriendRequestsAsync(string userId);
        Task<FriendRequest> GetFriendRequestByIdAsync(int id);
        Task AddFriendRequestAsync(FriendRequest friendRequest);
        Task<bool> AcceptFriendRequestAsync(int id);
        Task CancelFriendRequestAsync(int id);
        Task<FriendRequest> GetPendingRequestAsync(string senderId, string receiverId);
        Task<RequestStatus?> GetFriendRequestStatusAsync(string senderId, string receiverId);
    }
}
