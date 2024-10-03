public class FriendRequestController : Controller
{
    private readonly IFriendRequestService _friendRequestService;
    private readonly ILogger<FriendRequestController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public FriendRequestController(IFriendRequestService friendRequestService, ILogger<FriendRequestController> logger,
        UserManager<ApplicationUser> userManager)
    {
        _friendRequestService = friendRequestService;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetFriendRequestStatus(string receiverId)
    {
        try
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var status = await _friendRequestService.GetFriendRequestStatusAsync(currentUserId, receiverId);
            return Json(new { success = true, status = status.ToString() });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting friend request status");
            return Json(new { success = false, message = "An error occurred while getting the friend request status." });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendFriendRequest(string receiverId)
    {
        var senderId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(senderId))
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        var result = await _friendRequestService.SendFriendRequestAsync(senderId, receiverId);
        return Json(new { success = result });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelFriendRequest(string receiverId)
    {
        var senderId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(senderId))
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        var result = await _friendRequestService.CancelFriendRequestAsync(senderId, receiverId);
        return Json(new { success = result });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Accept([FromForm] string senderId, [FromForm] string requestId)
    {
        var receiverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(receiverId))
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        try
        {
            var result = await _friendRequestService.AcceptFriendRequestAsync(senderId, receiverId);
            return Json(new { success = result, message = result ? "Friend request accepted" : "Failed to accept friend request" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error accepting friend request");
            return Json(new { success = false, message = "An error occurred while processing the request" });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject([FromForm] string senderId, [FromForm] string requestId)
    {
        var receiverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(receiverId))
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        try
        {
            var result = await _friendRequestService.RejectFriendRequestAsync(senderId, receiverId);
            return Json(new { success = result, message = result ? "Friend request rejected" : "Failed to reject friend request" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting friend request");
            return Json(new { success = false, message = "An error occurred while processing the request" });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetReceivedFriendRequests()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendRequests = await _friendRequestService.GetFriendRequestsForUser(userId);

            var result = friendRequests.Select(r => new
            {
                senderId = r.SenderId,
                senderName = r.SenderName,
                id = r.Id
            });

            return Json(new { success = true, requests = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching received friend requests");
            return Json(new { success = false, message = "An error occurred while fetching friend requests." });
        }
    }
}
