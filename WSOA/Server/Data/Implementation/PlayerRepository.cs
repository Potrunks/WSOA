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

        public List<Player> GetPlayersByTournamentIdAndUserIds(int tournamentId, IEnumerable<int> userIds)
        {
            return (
                        from player in _dbContext.Players
                        where player.PlayedTournamentId == tournamentId
                                && userIds.Contains(player.UserId)
                        select player
                    )
                    .ToList();
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

        public List<PlayerPlayingDto> GetPlayerPlayingDtosByUserIdsAndTournamentId(IEnumerable<int> userIds, int tournamentId)
        {
            var raw = (
                    from player in _dbContext.Players
                    join user in _dbContext.Users on player.UserId equals user.Id
                    join tournament in _dbContext.Tournaments on player.PlayedTournamentId equals tournament.Id
                    join elimination in _dbContext.Eliminations on player.Id equals elimination.PlayerVictimId into left_elimination
                    from elimination in left_elimination.DefaultIfEmpty()
                    join bonus_earned in _dbContext.BonusTournamentEarneds on player.Id equals bonus_earned.PlayerId into left_bonus_earned
                    from bonus_earned in left_bonus_earned.DefaultIfEmpty()
                    join bonus in _dbContext.BonusTournaments on bonus_earned.BonusTournamentCode equals bonus.Code into left_bonus
                    from bonus in left_bonus.DefaultIfEmpty()
                    where userIds.Contains(user.Id) && tournament.Id == tournamentId
                    group new { user, elimination, bonus_earned, bonus } by player into grouped
                    select new
                    {
                        User = grouped.Select(gr => gr.user).Single(),
                        Player = grouped.Key,
                        Eliminations = grouped.Select(gr => gr.elimination),
                        BonusEarned = grouped.Select(gr => gr.bonus_earned),
                        Bonus = grouped.Select(gr => gr.bonus)
                    }
                )
                .ToList();

            return raw.Select(r => new PlayerPlayingDto
            {
                Id = r.Player.Id,
                FirstName = r.User.FirstName,
                LastName = r.User.LastName,
                TotalAddOn = r.Player.TotalAddOn,
                TotalRebuy = r.Player.TotalReBuy,
                IsEliminated = r.Eliminations.Any(eli => eli != null),
                BonusTournamentEarnedsByBonusTournamentCode = (
                    from b_earned in r.BonusEarned.Where(be => be != null)
                    join b in r.Bonus.Where(b => b != null) on b_earned.BonusTournamentCode equals b.Code
                    select new BonusTournamentEarnedDto
                    {
                        Code = b.Code,
                        Label = b.Label,
                        LogoPath = b.LogoPath,
                        Occurence = b_earned.Occurrence
                    }
                )
                .ToDictionary(sel => sel.Code)
            }).ToList();
        }
    }
}
