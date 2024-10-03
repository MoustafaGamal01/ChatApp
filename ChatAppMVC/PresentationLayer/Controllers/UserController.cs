using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace ChatAppMVC.PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFriendshipRepository _friendshipRepository;

        public UserController(IUserService userService, IFriendshipRepository friendshipRepository)
        {
            _userService = userService;
            _friendshipRepository = friendshipRepository;
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchChat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string username)
        {
            var searchUser = await _userService.GetUserByUsername(username);

            if (searchUser == null)
            {
                return View("SearchChat", new UserInfoViewModel());
            }

            var userDto = new UserInfoViewModel
            {
                UserId = searchUser.Id,
                DisplayName = searchUser.UserName,
                ProfilePictureUrl = searchUser.ProfilePictureUrl
            };

            var me = await _userService.GetUserByUsername(User.Identity.Name);
            var ok = await _friendshipRepository.GetFriendshipForTwoUsersAsync(me.Id, searchUser.Id);

            return View("AddUser", userDto);
        }
    }
}
