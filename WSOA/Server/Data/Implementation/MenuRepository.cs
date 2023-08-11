using WSOA.Server.Data.Interface;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Data.Implementation
{
    public class MenuRepository : IMenuRepository
    {
        private readonly WSOADbContext _dbContext;

        public MenuRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MainNavSectionViewModel> GetMainNavSectionVMsByProfileCode()
        {
            return
            (
                from mainNavSection in _dbContext.MainNavSections
                select new MainNavSectionViewModel
                {
                    ClassIcon = mainNavSection.ClassIcon,
                    Order = mainNavSection.Order
                }
            )
            .ToList();
        }
    }
}
