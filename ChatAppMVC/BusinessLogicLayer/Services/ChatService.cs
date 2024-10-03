using ChatAppMVC.BusinessLogicLayer.Services.IServices;
using ChatAppMVC.DataAccessLayer.Models;
using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;

namespace ChatAppMVC.BusinessLogicLayer.Services
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageService _messageService;
        private readonly IChatRepository _chatRepository;
        private readonly IChatUserRepository _chatUserRepository;

        public ChatService(IHubContext<ChatHub> hubContext, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            IMessageService messageService, IChatRepository chatRepository, IChatUserRepository chatUserRepository)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _messageService = messageService;
            _chatRepository = chatRepository;
            _chatUserRepository = chatUserRepository;
        }

        public async Task<ChatViewModel> OpenChatAsync(string userId, int chatId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId);
           
            var messages = await _messageService.GetMessagesByChatIdAsync(chatId);

            var otherUserId = chat.ChatUsers.First(u => u.UserId != userId).UserId;

            // users Info
            var currentUserInfo = await _userManager.FindByIdAsync(userId);
            var otherUserInfo = await _userManager.FindByIdAsync(otherUserId);

            var viewModel = new ChatViewModel
            {
                ChatId = chatId,
                CurrentUserId = userId,
                CurrentUser = new UserInfoViewModel
                {
                    UserId = currentUserInfo.Id,
                    DisplayName = currentUserInfo.UserName,
                    ProfilePictureUrl = currentUserInfo.ProfilePictureUrl
                },
                OtherUser = new UserInfoViewModel
                {
                    UserId = otherUserInfo.Id,
                    DisplayName = otherUserInfo.UserName,
                    ProfilePictureUrl = otherUserInfo.ProfilePictureUrl
                },
                Messages = messages.Select(m => new GetMessageDto
                {
                    SenderId = m.SenderId,
                    Content = m.Content,
                    Timestamp = m.SentAt,
                    MediaUrl = m.MediaUrl
                }).ToList()
            };

            return viewModel;
        }

        public async Task<GroupChatViewModel> CreateGroupChatAsync(string groupName, IFormFile file, List<string> userIds, string adminId)
        {
            string filePath = null;
            if (file != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                filePath = Path.Combine(uploads, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            var admin = await _userManager.FindByIdAsync(adminId);

            // Create chat users list
            List<ChatUser> chatUsers = new List<ChatUser>();

            // Add admin to chat users
            var adminChatUser = new ChatUser
            {
                DisplayName = groupName,
                Role = ChatUserRole.Admin,
                ChatPictureUrl = $"/images/{file.FileName}", 
                UserId = adminId,
                LastReadMessageId = null,
                ChatId = null
            };
            chatUsers.Add(adminChatUser);
            await _unitOfWork.ChatUserRepository.CreateChatUserAsync(adminChatUser);

            // other users chats
            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var otherUserChat = new ChatUser
                {
                    DisplayName = groupName,
                    Role = ChatUserRole.User,
                    ChatPictureUrl = $"/images/{file.FileName}",
                    UserId = userId,
                    LastReadMessageId = null,
                    ChatId = null
                };
                chatUsers.Add(otherUserChat);
                await _unitOfWork.ChatUserRepository.CreateChatUserAsync(otherUserChat);
            }
            //await _unitOfWork.CompleteAsync();

            // Create the chat
            var chat = new Chat
            {
                ChatName = groupName,
                Type = ChatType.Group,
                ChatUsers = chatUsers,
                ChatPictureUrl = $"/images/{file.FileName}" 
            };
            await _unitOfWork.ChatRepository.CreateChatAsync(chat);
            await _unitOfWork.CompleteAsync();

            // Create and return the GroupChatViewModel
            GroupChatViewModel groupChatView = new GroupChatViewModel
            {
                ChatId = chat.Id,
                ChatName = chat.ChatName,
                ChatType = chat.Type,
                ChatUsersIds = chatUsers.Select(cu => cu.UserId).ToList(),
                ChatPicture = $"/images/{file.FileName}"
            };

            return groupChatView;
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            return await _unitOfWork.ChatRepository.GetChatByIdAsync(chatId);
        }

        public async Task<GroupChatViewModel> OpenGroupChatAsync(string currentUserId, int chatId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId);
            if (chat == null || !chat.ChatUsers.Any(u => u.UserId == currentUserId))
            {
                return null;
            }

            var messages = await _messageService.GetMessagesByChatIdAsync(chatId);
            var otherUserId = chat.ChatUsers.First(u => u.UserId != currentUserId).UserId;

            var currentUserInfo = await _userManager.FindByIdAsync(currentUserId);

            var viewModel = new GroupChatViewModel
            {
                ChatId = chatId,
                ChatName = chat.ChatName,
                ChatPicture = chat.ChatPictureUrl,
                ChatType = chat.Type,
                CurrentUserId = currentUserId,
                ChatUsersIds = chat.ChatUsers.Select(cu => cu.UserId).ToList(),
                Messages = messages.Select(m => new GetMessageDto
                {
                    SenderId = m.SenderId,
                    SenderName = m.User.UserName,
                    Content = m.Content,
                    Timestamp = m.SentAt,
                    MediaUrl = m.MediaUrl
                }).ToList()
            };

            return viewModel;
        }

        public async Task JoinChatAsync(int chatId, string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, chatId.ToString());
        }

        public async Task LeaveChatAsync(int chatId, string connectionId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, chatId.ToString());
        }

        public async Task<List<GetChatUserViewModel>> LoadChatsAsync(string userId)
        {
            var chats = await _chatUserRepository.GetChatUsersByUserIdAsync(userId);

            var allChats = new List<GetChatUserViewModel>();
            var chatInfo = new GetChatUserViewModel();

            foreach (var chatUser in chats)
            {
                if (chatUser.ChatType == ChatType.Private)
                {
                    var chat = await _chatRepository.GetChatByIdAsync(chatUser.ChatId);
                    var otherUserId = chat.ChatUsers.First(u => u.UserId != userId);
                    chatUser.ChatId = chatUser.ChatId;
                    chatUser.ChatName = otherUserId.User.UserName;
                    chatUser.ChatPictureUrl = otherUserId.User.ProfilePictureUrl;
                    chatUser.LastMessage = chatUser.LastMessage;
                    chatUser.LastMessageTime = chatUser.LastMessageTime;
                }
                allChats.Add(chatUser);
            }

            return allChats;
        }
    }
}
