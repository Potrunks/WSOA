using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class BonusTournamentEarnedRepository : IBonusTournamentEarnedRepository
    {
        private readonly WSOADbContext _dbContext;

        public BonusTournamentEarnedRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BonusTournamentEarned SaveBonusTournamentEarned(BonusTournamentEarned bonusTournamentEarned)
        {
            BonusTournamentEarned? existingBonus = _dbContext.BonusTournamentEarneds.SingleOrDefault(b => b.BonusTournamentCode == bonusTournamentEarned.BonusTournamentCode && b.PlayerId == bonusTournamentEarned.PlayerId);
            if (existingBonus != null)
            {
                existingBonus.Occurrence++;
                _dbContext.SaveChanges();
                return existingBonus;
            }
            else
            {
                _dbContext.BonusTournamentEarneds.Add(bonusTournamentEarned);
                _dbContext.SaveChanges();
                return bonusTournamentEarned;
            }
        }
    }
}
