using log4net;
using WSOA.Server.Business.Interface;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Implementation
{
    public class MenuBusiness : IMenuBusiness
    {
        private readonly IMenuRepository _menuRepository;

        private readonly ILog _log = LogManager.GetLogger(nameof(MenuBusiness));

        public MenuBusiness(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public MainNavMenuResult LoadMainNavMenu()
        {
            List<MainNavSectionViewModel> mainNavSectionVMs = _menuRepository.GetMainNavSectionVMsByProfileCode();

            return new MainNavMenuResult(mainNavSectionVMs);
        }
    }
}
