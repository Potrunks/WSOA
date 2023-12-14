using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
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

        public IEnumerable<PlayerEliminationsDto> GetPlayerEliminationsDtosByPlayerVictimIds(IEnumerable<int> playerVictimIds)
        {
            var rawDatas =
            (
                from elim in _dbContext.Eliminations
                join elim_pla in _dbContext.Players on elim.PlayerVictimId equals elim_pla.Id
                join elim_usr in _dbContext.Users on elim_pla.UserId equals elim_usr.Id
                join killer_pla in _dbContext.Players on elim.PlayerEliminatorId equals killer_pla.Id
                join tou in _dbContext.Tournaments on elim_pla.PlayedTournamentId equals tou.Id
                where playerVictimIds.Contains(elim_pla.Id)
                group new { elim, killer_pla, tou, elim_usr } by elim_pla into grouped
                select new
                {
                    EliminatedPlayer = grouped.Key,
                    Eliminations = grouped.Select(gr => gr.elim),
                    EliminatorPlayers = grouped.Select(gr => gr.killer_pla).Distinct(),
                    Tournament = grouped.Select(gr => gr.tou).Distinct(),
                    EliminatedUser = grouped.Select(gr => gr.elim_usr).Distinct()
                }
            )
            .ToList();

            return rawDatas.Select(data => new PlayerEliminationsDto
            {
                EliminatedPlayer = data.EliminatedPlayer,
                Eliminations = data.Eliminations,
                EliminatorPlayersById = data.EliminatorPlayers.ToDictionary(pla => pla.Id),
                Tournament = data.Tournament.Single(),
                EliminatedUser = data.EliminatedUser.Single()
            });
        }

        public void DeleteElimination(Elimination elimination)
        {
            _dbContext.Eliminations.Remove(elimination);
            _dbContext.SaveChanges();
        }
    }
}
