using ChatAppMVC.DataAccessLayer.Models;
using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace ChatAppMVC.PresentationLayer.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IChatUserService _chatUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public ChatController(IChatService chatService, IMessageService messageService, IChatUserService chatUserService,
            UserManager<ApplicationUser> userManager, IFriendshipRepository friendshipRepository, 
            IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _chatService = chatService;
            _messageService = messageService;
            _chatUserService = chatUserService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadChats()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            var allChats =  await _chatService.LoadChatsAsync(userId);
            
            return View("Index", allChats);
        }

        [HttpGet]
        public async Task<IActionResult> OpenChat(int chatId)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }
            
            var viewModel = await _chatService.OpenChatAsync(currentUserId, chatId);
            
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OpenGroupChat(int chatId)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            var viewModel = await _chatService.OpenGroupChatAsync(currentUserId, chatId);

            if(viewModel == null)
            {
                return NotFound("Chat not found or user not authorized");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateGroup()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            var friendsWithUser = await _unitOfWork.FriendshipRepository.GetUserFriends(userId);

            ViewBag.Friends = friendsWithUser;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromForm] GroupChatViewModel model, IFormFile file)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }
            
            var chat = await _chatService.CreateGroupChatAsync(model.ChatName, file, model.ChatUsersIds, currentUserId);

            foreach (var userId in model.ChatUsersIds)
            {
                await _notificationService.SendNotificationAsync(userId, currentUserId, $" Added you to group {model.ChatName}");
            }

            return RedirectToAction("LoadChats");
		}
    }
}
