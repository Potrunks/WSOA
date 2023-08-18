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

        public IDictionary<MainNavSection, List<MainNavSubSection>> GetMainNavSubSectionsBySectionAndProfileCode(string profileCode)
        {
            return
            (
                from mns in _dbContext.MainNavSections
                join ss in _dbContext.MainNavSubSections on mns.Id equals ss.MainNavSectionId into left_ss
                from ss in left_ss.DefaultIfEmpty()
                join ss_pc in _dbContext.MainNavSubSectionsByProfileCode on ss.Id equals ss_pc.MainNavSubSectionId into left_ss_pc
                from ss_pc in left_ss_pc.DefaultIfEmpty()
                where
                    ss_pc.ProfileCode == profileCode
                    || ss.Id == null
                group new { mns, ss } by mns into grouped
                select new
                {
                    MainNavSection = grouped.Key,
                    MainNavSubSections = grouped.All(g => g.ss == null) ? new List<MainNavSubSection>() : grouped.Select(x => x.ss).ToList()
                }
            ).ToDictionary(x => x.MainNavSection, x => x.MainNavSubSections);
        }
    }
}
