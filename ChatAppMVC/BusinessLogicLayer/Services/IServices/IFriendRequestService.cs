namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public interface IFriendRequestService
    {
        Task<bool> SendFriendRequestAsync(string senderId, string receiverId);
        Task<bool> CancelFriendRequestAsync(string senderId, string receiverId);
        Task<bool> AcceptFriendRequestAsync(string senderId, string receiverId);
        Task<bool> RejectFriendRequestAsync(string senderId, string receiverId);
        Task<IEnumerable<GetFriendRequestDto>> GetFriendRequestsForUser(string userId);
        Task<RequestStatus?> GetFriendRequestStatusAsync(string senderId, string receiverId);
    }
}
