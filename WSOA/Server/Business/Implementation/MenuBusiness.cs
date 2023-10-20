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

        public APICallResult<MainNavMenuViewModel> LoadMainNavMenu(ISession currentSession)
        {
            APICallResult<MainNavMenuViewModel> result = new APICallResult<MainNavMenuViewModel>(true);

            try
            {
                string currentProfileCode = currentSession.GetString(HttpSessionResources.KEY_PROFILE_CODE);
                if (currentProfileCode == null)
                {
                    string errorMsg = MainBusinessResources.USER_NOT_CONNECTED;
                    return new APICallResult<MainNavMenuViewModel>(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
                }

                IDictionary<MainNavSection, List<MainNavSubSection>> subSectionsBySection = _menuRepository.GetMainNavSubSectionsInSectionByProfileCode(currentProfileCode);
                if (subSectionsBySection.IsNullOrEmpty() || subSectionsBySection.Values.All(ss => ss.IsNullOrEmpty()))
                {
                    throw new Exception(string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, nameof(subSectionsBySection), nameof(MenuBusiness.LoadMainNavMenu)));
                }

                result.Data = new MainNavMenuViewModel(subSectionsBySection);
            }
            catch (Exception exception)
            {
                _log.Error(string.Format(MenuBusinessResources.LOAD_MENU_TECH_ERROR, exception.Message));
                string errorMsg = MainBusinessResources.TECHNICAL_ERROR;
                return new APICallResult<MainNavMenuViewModel>(errorMsg, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsg));
            }

            return result;
        }
    }
}
