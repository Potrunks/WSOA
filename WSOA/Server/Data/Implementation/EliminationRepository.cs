using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class EliminationRepository : IEliminationRepository
    {
        private readonly WSOADbContext _dbContext;

        public EliminationRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Elimination SaveElimination(Elimination elimination)
        {
            if (elimination.Id == 0)
            {
                _dbContext.Eliminations.Add(elimination);
            }
            _dbContext.SaveChanges();
            return elimination;
        }

        public IEnumerable<Elimination> GetEliminationsByPlayerVictimIds(IEnumerable<int> playerVictimIds)
        {
            return
                (
                    from elim in _dbContext.Eliminations
                    where playerVictimIds.Contains(elim.PlayerVictimId)
                    select elim
                );
        }
    }
}
