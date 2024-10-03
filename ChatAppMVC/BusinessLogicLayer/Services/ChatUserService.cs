

using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;

namespace ChatAppMVC.BusinessLogicLayer.Services
{
    public class ChatUserService : IChatUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<FriendRequestHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatUserService(IUnitOfWork unitOfWork, IHubContext<FriendRequestHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<GetChatUserViewModel>> GetChatUsersByUserId(string userId)
        {
            return await _unitOfWork.ChatUserRepository.GetChatUsersByUserIdAsync(userId);
        }


        public async Task<ChatUser>GetChatUserByIdAsync(int chatUserId)
        {
            return await _unitOfWork.ChatUserRepository.GetChatUserByIdAsync(chatUserId);
        }
    }
}
