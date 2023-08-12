using WSOA.Shared.Result;

namespace WSOA.Server.Business.Interface
{
    public interface IMenuBusiness
    {
        /// <summary>
        /// Load Main nav menu for the user connected.
        /// </summary>
        MainNavMenuResult LoadMainNavMenu();
    }
}
