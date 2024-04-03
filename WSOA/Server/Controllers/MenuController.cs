using Microsoft.AspNetCore.Mvc;
using WSOA.Server.Business.Interface;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Controllers
{
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuBusiness _menuBusiness;

        public MenuController(IMenuBusiness menuBusiness)
        {
            _menuBusiness = menuBusiness;
        }

        [HttpGet]
        [Route("api/menu/loadMainNavMenu")]
        public APICallResult<MainNavMenuViewModel> LoadMainNavMenu()
        {
            return _menuBusiness.LoadMainNavMenu(HttpContext.Session);
        }
    }
}
