using WSOA.Shared.ViewModel;

namespace WSOA.Server.Data.Interface
{
    public interface IMenuRepository
    {
        /// <summary>
        /// Get main nav section view models.
        /// </summary>
        List<MainNavSectionViewModel> GetMainNavSectionVMsByProfileCode();
    }
}
