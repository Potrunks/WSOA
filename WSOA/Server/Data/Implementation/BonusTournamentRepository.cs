using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class BonusTournamentRepository : IBonusTournamentRepository
    {
        private readonly WSOADbContext _dbContext;

        public BonusTournamentRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BonusTournament> GetAll()
        {
            return _dbContext.BonusTournaments;
        }
    }
}
