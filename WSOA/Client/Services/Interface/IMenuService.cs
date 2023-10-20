using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Interface
{
    public interface IMenuService
    {
        /// <summary>
        /// Load the main nav menu for the current user.
        /// </summary>
        Task<APICallResult<MainNavMenuViewModel>> LoadMainMenu();
    }
}
