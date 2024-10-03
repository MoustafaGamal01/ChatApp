
namespace ChatAppMVC.BusinessLogicLayer.Services.IServices
{
    public interface IRoleService
    {
        Task<List<string>> AddRole(RoleViewModel roleVM);
    }
}
