using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppMVC.PresentationLayer.Controllers
{
    public class NotificationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(UserManager<ApplicationUser> userManager, INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                var notifications = await _notificationService.GetUserNotificationsAsync(userId, 1);

                var formattedNotifications = notifications.Select(n => new
                {
                    content = n.Content,
                    createdAt = n.CreatedAt.ToString("o") 
                });

                return Json(new { success = true, notifications = formattedNotifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching notifications");
                return StatusCode(500, new { success = false, message = "An error occurred while fetching notifications" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                await _notificationService.MarkAllNotificationsAsReadAsync(userId);
                return Json(new { success = true, message = "All notifications marked as read" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while marking notifications as read");
                return StatusCode(500, new { success = false, message = "An error occurred while marking notifications as read" });
            }
        }
    }
}
