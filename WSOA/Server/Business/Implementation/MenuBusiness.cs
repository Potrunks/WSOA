using log4net;
using Microsoft.IdentityModel.Tokens;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
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

        public MainNavMenuResult LoadMainNavMenu(ISession currentSession)
        {
            MainNavMenuResult result = new MainNavMenuResult(null);

            try
            {
                string currentProfileCode = currentSession.GetString(HttpSessionResources.KEY_PROFILE_CODE);
                if (currentProfileCode == null)
                {
                    string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                    return new MainNavMenuResult(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                List<MainNavSection> mainNavSections = _menuRepository.GetMainNavSections();
                if (mainNavSections.IsNullOrEmpty())
                {
                    throw new Exception(string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, mainNavSections, nameof(MenuBusiness.LoadMainNavMenu)));
                }

                List<MainNavSubSection> mainNavSubSections = _menuRepository.GetMainNavSubSectionsByProfileCode(currentProfileCode);
                if (mainNavSubSections.IsNullOrEmpty())
                {
                    throw new Exception(string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, mainNavSections, nameof(MenuBusiness.LoadMainNavMenu)));
                }

                foreach (MainNavSection mainNavSection in mainNavSections)
                {
                    MainNavSectionViewModel mainNavSectionVM = new MainNavSectionViewModel(mainNavSection);
                    mainNavSectionVM.MainNavSubSectionVMs = mainNavSubSections.Where(sub => sub.MainNavSectionId == mainNavSection.Id).Select(sub => new MainNavSubSectionViewModel(sub)).ToList();
                    result.MainNavSectionVMs.Add(mainNavSectionVM);
                }
            }
            catch (Exception exception)
            {
                _log.Error(string.Format(MenuBusinessResources.LOAD_MENU_TECH_ERROR, exception.Message));
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new MainNavMenuResult(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }

            return result;
        }
    }
}
