
namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public enum ResultEnum
    {
        InvalidEmailOrPassword,
        Done
    }
    public interface IAuthService
    {
        Task<List<string>> Register(RegisterViewModel customerRegisterVM);

        Task<ResultEnum> Login(LoginViewModel loginVM);

        Task<ResultEnum> Logout();
    }
}
