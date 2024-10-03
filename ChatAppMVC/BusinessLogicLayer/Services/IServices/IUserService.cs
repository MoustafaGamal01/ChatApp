namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public interface IUserService
    {
        Task <ApplicationUser> GetUserByUsername(string username);
    }
}
