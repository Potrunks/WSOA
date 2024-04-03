using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Interface
{
    public interface IMenuBusiness
    {
        /// <summary>
        /// Load Main nav menu for the user connected.
        /// </summary>
        APICallResult<MainNavMenuViewModel> LoadMainNavMenu(ISession currentSession);
    }
}
