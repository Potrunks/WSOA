using WSOA.Shared.Result;

namespace WSOA.Client.Services.Interface
{
    public interface IMenuService
    {
        /// <summary>
        /// Load the main nav menu for the current user.
        /// </summary>
        Task<MainNavMenuResult> LoadMainMenu();
    }
}
