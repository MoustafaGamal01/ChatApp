using Microsoft.AspNetCore.Mvc;

namespace ChatAppMVC.PresentationLayer.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(IFormFile file, string messageText, int chatId, string senderId)
        {
            if (string.IsNullOrEmpty(senderId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            await _messageService.SendMessageAsync(chatId, senderId, messageText, file);

            return Json(new { success = true });
        }
    }
}
