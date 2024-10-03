namespace ChatAppMVC.DataAccessLayer.Models.Hubs
{
    public class FriendRequestHub : Hub
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestHub(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        public async Task SendFriendRequest(string receiverUsername, string senderUsername)
        {
            await _friendRequestService.SendFriendRequestAsync(senderUsername, receiverUsername);
        }

        public async Task AcceptFriendRequest(string senderId, string recId)
        {
            await _friendRequestService.AcceptFriendRequestAsync(senderId, recId);
        }

        public async Task CancelFriendRequest(string senderId, string recId)
        {
            await _friendRequestService.CancelFriendRequestAsync(senderId, recId);
        }
    }

}
