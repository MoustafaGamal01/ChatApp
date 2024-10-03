using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ChatAppMVC.BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<string>> Register(RegisterViewModel customerRegisterVM)
        {
            List<string> answer = new List<string>();


            string PicName = null;
            if(customerRegisterVM.ProfilePicture != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var filePath = Path.Combine(uploads, customerRegisterVM.ProfilePicture.FileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await customerRegisterVM.ProfilePicture.CopyToAsync(fileStream);
                }
            }


            var isFound = await _userManager.FindByNameAsync(customerRegisterVM.UserName);
            if (isFound != null)
            {
                answer.Add("UsernameExists");
                return answer;
            }

            ApplicationUser user = new ApplicationUser();
            user.UserName = customerRegisterVM.UserName;
            user.PasswordHash = customerRegisterVM.Password;
            user.ProfilePictureUrl = customerRegisterVM.ProfilePicture.FileName != null ? customerRegisterVM.ProfilePicture.FileName : "EmptyProfilePic.jpeg";
			var result = await _userManager.CreateAsync(user, customerRegisterVM.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                foreach (var res in result.Errors)
                {
                    answer.Add(res.Description);
                }
            }
            return answer;

        }

        public async Task<ResultEnum> Login(LoginViewModel loginVM)
        {
            // check email
            ApplicationUser userModel = await _userManager.FindByNameAsync(loginVM.Username);

            if (userModel != null)
            {
                // check password
                var validPassword = await _userManager.CheckPasswordAsync(userModel, loginVM.Password);

                if (validPassword)
                {
                    // signIn
                    await _signInManager.SignInAsync(userModel, loginVM.RememberMe);

                    return ResultEnum.Done;
                }

            }
            return ResultEnum.InvalidEmailOrPassword;
        }

        public async Task<ResultEnum> Logout()
        {
            await _signInManager.SignOutAsync();
            return ResultEnum.Done;
        }
    }
}
