using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class JackpotDistributionRepository : IJackpotDistributionRepository
    {
        private readonly WSOADbContext _dbContext;

        public JackpotDistributionRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveJackpotDistributions(IEnumerable<JackpotDistribution> jackpotDistributions)
        {
            IEnumerable<JackpotDistribution> newJackpotDistributions = jackpotDistributions.Where(jac => jac.Id == 0);
            if (newJackpotDistributions.Any())
            {
                _dbContext.JackpotDistributions.AddRange(newJackpotDistributions);
            }
            _dbContext.SaveChanges();
        }

        public List<JackpotDistribution> GetJackpotDistributionsByTournamentId(int tournamentId)
        {
            return _dbContext.JackpotDistributions.Where(jac => jac.TournamentId == tournamentId).ToList();
        }

        public void RemoveJackpotDistributions(IEnumerable<JackpotDistribution> jackpotDistributions)
        {
            _dbContext.JackpotDistributions.RemoveRange(jackpotDistributions);
            _dbContext.SaveChanges();
        }
    }
}
