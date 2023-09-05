using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly WSOADbContext _dbContext;

        public TournamentRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveTournament(Tournament tournament)
        {
            if (tournament.Id == 0)
            {
                _dbContext.Tournaments.Add(tournament);
            }
            _dbContext.SaveChanges();
        }
    }
}
