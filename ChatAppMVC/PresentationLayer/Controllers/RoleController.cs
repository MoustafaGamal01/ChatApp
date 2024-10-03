
namespace ChatAppMVC.PresentationLayer.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveAddRole(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                List<string> result = await _roleService.AddRole(roleVM);

                if(result.Count == 0)
                {
                    return View("AddRole", new RoleViewModel());
                }
                foreach (var item in result)
                {
                    ModelState.AddModelError("", item);
                }
            }
            return View("AddRole", roleVM);
        }

    }
}
