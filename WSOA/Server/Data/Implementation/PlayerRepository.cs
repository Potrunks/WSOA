using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Implementation
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly WSOADbContext _dbContext;

        public PlayerRepository(WSOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Player? GetPlayerByTournamentIdAndUserId(int tournamentId, int userId)
        {
            return
            (
                from pla in _dbContext.Players
                where pla.PlayedTournamentId == tournamentId
                      && pla.UserId == userId
                select pla
            )
            .SingleOrDefault();
        }

        public IEnumerable<PlayerDto> GetPlayersByTournamentIdAndPresenceStateCode(int tournamentId, string presenceStateCode)
        {
            return
                (
                    from usr in _dbContext.Users
                    join pla in _dbContext.Players on usr.Id equals pla.UserId
                    where pla.PlayedTournamentId == tournamentId
                          && pla.PresenceStateCode == presenceStateCode
                    select new PlayerDto
                    {
                        Player = pla,
                        User = usr
                    }
                );
        }

        public void SavePlayer(Player player)
        {
            if (player.Id == 0)
            {
                _dbContext.Players.Add(player);
            }
            _dbContext.SaveChanges();
        }

        public void SavePlayers(IEnumerable<Player> players)
        {
            IEnumerable<Player> newPlayers = players.Where(pla => pla.Id == 0);
            if (newPlayers.Any())
            {
                _dbContext.Players.AddRange(newPlayers);
            }
            _dbContext.SaveChanges();
        }

        public void DeletePlayers(IEnumerable<Player> players)
        {
            _dbContext.Players.RemoveRange(players);
            _dbContext.SaveChanges();
        }

        public IDictionary<int, IEnumerable<BonusTournamentEarned>> GetBonusTournamentEarnedsByPlayerIds(IEnumerable<int> playerIds)
        {
            return (
                        from bte in _dbContext.BonusTournamentEarneds
                        where playerIds.Contains(bte.PlayerId)
                        select bte
                    )
                    .GroupBy(bte => bte.PlayerId)
                    .Select(g => new
                    {
                        PlayerId = g.Key,
                        BonusTournamentEarneds = g.Select(bte => bte)
                    })
                    .ToDictionary(sel => sel.PlayerId, sel => sel.BonusTournamentEarneds);
        }

        public IDictionary<int, Player> GetPlayersByIds(IEnumerable<int> playerIds)
        {
            return
                (
                    from pla in _dbContext.Players
                    where playerIds.Contains(pla.Id)
                    select pla
                )
                .ToDictionary(pla => pla.Id);
        }

        public IEnumerable<PlayerDto> GetPlayerDtosByPlayerIds(IEnumerable<int> playerIds)
        {
            return (
                    from pla in _dbContext.Players
                    join usr in _dbContext.Users on pla.UserId equals usr.Id
                    join tou in _dbContext.Tournaments on pla.PlayedTournamentId equals tou.Id
                    join bon in _dbContext.BonusTournamentEarneds on pla.Id equals bon.PlayerId into left_bon
                    from bon in left_bon.DefaultIfEmpty()
                    join elim_as_elim in _dbContext.Eliminations on pla.Id equals elim_as_elim.PlayerEliminatorId into left_elim_as_elim
                    from elim_as_elim in left_elim_as_elim.DefaultIfEmpty()
                    join elim_as_victim in _dbContext.Eliminations on pla.Id equals elim_as_victim.PlayerVictimId into left_elim_as_victim
                    from elim_as_victim in left_elim_as_victim.DefaultIfEmpty()
                    where playerIds.Contains(pla.Id)
                    group new { usr, tou, elim_as_elim, elim_as_victim, bon } by pla into grouped
                    select new PlayerDto
                    {
                        Player = grouped.Key,
                        User = grouped.First().usr,
                        Tournament = grouped.First().tou,
                        BonusTournamentEarneds = grouped.Where(gr => gr.bon != null).Select(gr => gr.bon),
                        EliminationsAsEliminator = grouped.Where(gr => gr.elim_as_elim != null).Select(gr => gr.elim_as_elim),
                        EliminationsAsVictim = grouped.Where(gr => gr.elim_as_victim != null).Select(gr => gr.elim_as_victim)
                    }
                );
        }
    }
}
