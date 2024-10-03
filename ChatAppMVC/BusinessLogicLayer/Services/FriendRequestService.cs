using ChatAppMVC.DataAccessLayer.Models;
using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatAppMVC.BusinessLogicLayer.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<FriendRequestHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public FriendRequestService(IUnitOfWork unitOfWork, IHubContext<FriendRequestHub> hubContext, UserManager<ApplicationUser> userManager,
             INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<GetFriendRequestDto>> GetFriendRequestsForUser(string userId)
        {
            var requests = await _unitOfWork.FriendRequestRepository.GetFriendRequestsAsync(userId);
            var detailedRequests = new List<GetFriendRequestDto>();

            foreach (var request in requests)
            {
                var sender = await _userManager.FindByIdAsync(request.SenderId);
                detailedRequests.Add(new GetFriendRequestDto
                {
                    Id = request.Id,
                    SenderId = request.SenderId,
                    SenderName = sender?.UserName ?? "Unknown User",
                    CreatedAt = request.CreatedAt
                });
            }

            return detailedRequests;
        }

        public async Task<bool> SendFriendRequestAsync(string senderId, string receiverId)
        {
            try
            {
                var friendRequest = new FriendRequest
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Status = RequestStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.FriendRequestRepository.AddFriendRequestAsync(friendRequest);
                await _unitOfWork.CompleteAsync();

                var sender = await _userManager.FindByIdAsync(senderId);
                if (sender == null)
                {
                    return false;
                }

                await _hubContext.Clients.User(receiverId).SendAsync("ReceiveFriendRequest", new
                {
                    senderId = senderId,
                    senderName = sender.UserName,
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CancelFriendRequestAsync(string senderId, string receiverId)
        {
            var request = await _unitOfWork.FriendRequestRepository.GetPendingRequestAsync(senderId, receiverId);
            if (request == null)
                return false;

            await _unitOfWork.FriendRequestRepository.CancelFriendRequestAsync(request.Id);
            await _unitOfWork.CompleteAsync();

            // Notify the sender about cancellation
            await _hubContext.Clients.User(senderId).SendAsync("FriendRequestCancelled", receiverId);
            return true;
        }

        public async Task<bool> AcceptFriendRequestAsync(string senderId, string receiverId)
        {
            var request = await _unitOfWork.FriendRequestRepository.GetPendingRequestAsync(senderId, receiverId);
            if (request == null)
                return false;

            // delete request 
            await _unitOfWork.FriendRequestRepository.CancelFriendRequestAsync(request.Id);

            // get sender and receiver
            var sender = await _userManager.FindByIdAsync(senderId);
            var receiver = await _userManager.FindByIdAsync(receiverId);

            // adding friendship
            var friendship = new Friendship
            {
                User1Id = senderId,
                User2Id = receiverId,
            };
            await _unitOfWork.FriendshipRepository.AddFriendshipAsync(friendship);

            // add chat for sender
            var userChat1 = new ChatUser
            {
                DisplayName = receiver.UserName,
                Role = ChatUserRole.User,
                ChatPictureUrl = sender.ProfilePictureUrl,
                UserId = senderId,
                LastReadMessageId = null,
                ChatId = null
            };
            // add chat user for receiver
            var userChat2 = new ChatUser
            {
                DisplayName = sender.UserName,
                Role = ChatUserRole.User,
                ChatPictureUrl = receiver.ProfilePictureUrl,
                UserId = receiverId,
                LastReadMessageId = null,
                ChatId = null
            };

            await _unitOfWork.ChatUserRepository.CreateChatUserAsync(userChat1);
            await _unitOfWork.ChatUserRepository.CreateChatUserAsync(userChat2);
            await _unitOfWork.CompleteAsync();

            // add chat with the users 
            var chat = new Chat
            {
                ChatName = $"{sender.UserName} and {receiver.UserName}",
                Type = ChatType.Private,
                ChatUsers = new List<ChatUser> { userChat1, userChat2 }
            };
            await _unitOfWork.ChatRepository.CreateChatAsync(chat);

            // Notify the sender about acceptance
            await _notificationService.SendNotificationAsync(senderId, receiverId, $" accepted your friend request");
            return true;
        }

        public async Task<bool> RejectFriendRequestAsync(string senderId, string receiverId)
        {
            var request = await _unitOfWork.FriendRequestRepository.GetPendingRequestAsync(senderId, receiverId);
            if (request == null)
                return false;

            await _unitOfWork.FriendRequestRepository.CancelFriendRequestAsync(request.Id);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<RequestStatus?> GetFriendRequestStatusAsync(string senderId, string receiverId)
        {
            return await _unitOfWork.FriendRequestRepository.GetFriendRequestStatusAsync(senderId, receiverId);
        }
    }
}
