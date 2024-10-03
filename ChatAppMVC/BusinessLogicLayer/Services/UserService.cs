
namespace ChatAppMVC.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            return await userManager.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        }
    }
}
