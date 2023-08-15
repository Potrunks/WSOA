using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class MenuRepository : IMenuRepository
    {
        private readonly WSOADbContext _dbContext;

        public MenuRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MainNavSection> GetMainNavSections()
        {
            return
            (
                from mns in _dbContext.MainNavSections
                select mns
            )
            .ToList();
        }

        public MainNavSubSection? GetMainNavSubSectionByIdAndProfileCode(string profileCode, int id)
        {
            return
            (
                from ss in _dbContext.MainNavSubSections
                join ss_pc in _dbContext.MainNavSubSectionsByProfileCode on ss.Id equals ss_pc.MainNavSubSectionId
                where
                    ss.Id == id
                    && ss_pc.ProfileCode == profileCode
                select ss
            )
            .SingleOrDefault();
        }

        public List<MainNavSubSection> GetMainNavSubSectionsByProfileCode(string profileCode)
        {
            return
            (
                from mnss in _dbContext.MainNavSubSections
                join mnss_pc in _dbContext.MainNavSubSectionsByProfileCode on mnss.Id equals mnss_pc.MainNavSubSectionId
                where mnss_pc.ProfileCode == profileCode
                select mnss
            )
            .ToList();
        }
    }
}
