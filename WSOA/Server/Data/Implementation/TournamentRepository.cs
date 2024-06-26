﻿using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;

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

        public List<TournamentDto> GetTournamentDtosByIsOverAndIsInProgress(bool isOver, bool isInProgress)
        {
            return
            (
                from tournament in _dbContext.Tournaments
                join player in _dbContext.Players on tournament.Id equals player.PlayedTournamentId into left_player
                from player in left_player.DefaultIfEmpty()
                join usr in _dbContext.Users on player.UserId equals usr.Id into left_usr
                from usr in left_usr.DefaultIfEmpty()
                join address in _dbContext.Addresses on tournament.AddressId equals address.Id
                where tournament.IsOver == isOver
                        && tournament.IsInProgress == isInProgress
                group new { player, usr, address } by tournament into grouped
                select new TournamentDto
                {
                    Tournament = grouped.Key,
                    Players = grouped.Where(g => g.player != null)
                                     .Select(g => new PlayerDto
                                     {
                                         Player = g.player,
                                         User = g.usr
                                     }),
                    Address = grouped.First().address
                }
            )
            .ToList();
        }

        public TournamentDto GetTournamentDtoById(int tournamentId)
        {
            return
            (
                from tournament in _dbContext.Tournaments
                join player in _dbContext.Players on tournament.Id equals player.PlayedTournamentId into left_player
                from player in left_player.DefaultIfEmpty()
                join usr in _dbContext.Users on player.UserId equals usr.Id into left_usr
                from usr in left_usr.DefaultIfEmpty()
                join address in _dbContext.Addresses on tournament.AddressId equals address.Id
                where tournament.Id == tournamentId
                group new { player, usr, address } by tournament into grouped
                select new TournamentDto
                {
                    Tournament = grouped.Key,
                    Players = grouped.Where(g => g.player != null)
                                     .Select(g => new PlayerDto
                                     {
                                         Player = g.player,
                                         User = g.usr
                                     }),
                    Address = grouped.First().address
                }
            )
            .Single();
        }

        public Tournament GetTournamentById(int tournamentId)
        {
            return
            (
                from tournament in _dbContext.Tournaments
                where tournament.Id == tournamentId
                select tournament
            )
            .Single();
        }

        public bool ExistsTournamentByTournamentIdIsOverAndIsInProgress(int tournamentId, bool isOver, bool isInProgress)
        {
            return
                (
                    from tou in _dbContext.Tournaments
                    where tou.Id == tournamentId
                            && tou.IsOver == isOver
                            && tou.IsInProgress == isInProgress
                    select tou
                )
                .Any();
        }

        public bool ExistsTournamentByIsInProgress(bool isInProgress)
        {
            return
                (
                    from tou in _dbContext.Tournaments
                    where tou.IsInProgress == isInProgress
                    select tou
                )
                .Any();
        }

        public Tournament? GetTournamentInProgress()
        {
            return _dbContext.Tournaments.SingleOrDefault(tou => tou.IsInProgress);
        }

        public int GetTournamentNumber(Tournament tournament)
        {
            List<Tournament> tournaments = _dbContext.Tournaments.Where(tou => tou.Season == tournament.Season && (tou.IsOver || tou.IsInProgress))
                                                                 .OrderBy(tou => tou.StartDate)
                                                                 .ToList();

            return tournaments.IndexOf(tournament) + 1;
        }

        public Tournament? GetPreviousTournament(Tournament currentTournament)
        {
            string previousSeason = string.Empty;
            if (int.TryParse(currentTournament.Season, out int parseResult))
            {
                previousSeason = (int.Parse(currentTournament.Season) - 1).ToString();
            }

            return _dbContext.Tournaments.Where(tou => ((currentTournament.Season == SeasonResources.OUT_OF_SEASON && currentTournament.Season == tou.Season)
                                                         || (currentTournament.Season != SeasonResources.OUT_OF_SEASON && (tou.Season == currentTournament.Season || tou.Season == previousSeason)))
                                                       && tou.IsOver
                                                       && tou.Id != currentTournament.Id
                                         )
                                         .OrderByDescending(tou => tou.StartDate)
                                         .FirstOrDefault();
        }

        public Tournament? GetLastFinishedTournamentBySeason(string season)
        {
            return _dbContext.Tournaments.Where(tou => tou.Season == season && tou.IsOver)
                                         .OrderByDescending(tou => tou.StartDate)
                                         .FirstOrDefault();
        }

        public TournamentToCancelDto GetTournamentToCancelDtoByTournamentId(int tournamentToCancelId)
        {
            return
                (
                    from tournament in _dbContext.Tournaments
                    join player in _dbContext.Players on tournament.Id equals player.PlayedTournamentId into left_player
                    from player in left_player.DefaultIfEmpty()
                    join elim in _dbContext.Eliminations on player.Id equals elim.PlayerEliminatorId into left_elim
                    from elim in left_elim.DefaultIfEmpty()
                    join bonus in _dbContext.BonusTournamentEarneds on player.Id equals bonus.PlayerId into left_bonus
                    from bonus in left_bonus.DefaultIfEmpty()
                    where tournament.Id == tournamentToCancelId
                    group new { player, elim, bonus } by tournament into grouped
                    select new TournamentToCancelDto
                    {
                        TournamentToCancel = grouped.Key,
                        PlayersToUpdate = grouped.Select(g => g.player).Where(p => p != null),
                        BonusToDelete = grouped.Select(g => g.bonus).Where(b => b != null),
                        EliminationsToDelete = grouped.Select(g => g.elim).Where(e => e != null)
                    }
                )
                .Single();
        }

        public void DeleteTournaments(IEnumerable<Tournament> tournamentsToDelete)
        {
            _dbContext.Tournaments.RemoveRange(tournamentsToDelete);
            _dbContext.SaveChanges();
        }

        public List<TournamentPlayedDto> LoadTournamentPlayedDtos(string season)
        {
            var rawResults = (
                    from tou in _dbContext.Tournaments
                    join pla in _dbContext.Players on tou.Id equals pla.PlayedTournamentId
                    join usr in _dbContext.Users on pla.UserId equals usr.Id
                    where
                        tou.IsOver
                        && tou.Season == season
                        && pla.PresenceStateCode == PresenceStateResources.PRESENT_CODE
                    group new { pla, usr } by tou into grouped
                    select new
                    {
                        grouped.Key.StartDate,
                        grouped.Key.BuyIn,
                        Players = grouped.Select(g => g.pla),
                        Users = grouped.Select(g => g.usr)
                    }
                ).ToList();

            List<TournamentPlayedDto> results = rawResults.Select(rr => new TournamentPlayedDto
            {
                StartDate = rr.StartDate,
                BuyIn = rr.BuyIn,
                PlayerResults = rr.Players.Select(p => new PlayerResultDto
                {
                    PlayerId = p.Id,
                    FirstName = rr.Users.Single(u => u.Id == p.UserId).FirstName,
                    LastName = rr.Users.Single(u => u.Id == p.UserId).LastName,
                    Position = p.CurrentTournamentPosition!.Value,
                    Points = p.TotalWinningsPoint!.Value,
                    UserId = rr.Users.Single(u => u.Id == p.UserId).Id,
                    TotalAddon = p.TotalAddOn.HasValue ? p.TotalAddOn.Value : 0,
                    TotalRebuy = p.TotalReBuy.HasValue ? p.TotalReBuy.Value : 0,
                    BuyIn = rr.BuyIn,
                    TotalWinningAmount = p.TotalWinningsAmount.HasValue ? p.TotalWinningsAmount.Value : 0,
                    WasFinalTable = p.WasFinalTable.HasValue && p.WasFinalTable.Value ? true : false,
                    PresenceStateCode = p.PresenceStateCode
                }).ToList()
            }).ToList();

            IEnumerable<int> allPlayerIds = results.SelectMany(r => r.PlayerResults).Select(p => p.PlayerId).Distinct();

            List<BonusTournamentEarnedResultDto> bonus = _dbContext.BonusTournamentEarneds.Where(bon => allPlayerIds.Contains(bon.PlayerId)).Select(b => new BonusTournamentEarnedResultDto
            {
                PlayerId = b.PlayerId,
                Code = b.BonusTournamentCode,
                Occurence = b.Occurrence,
                Points = b.PointAmount
            }).ToList();

            List<EliminationResultDto> eliminations = (
                                from eli in _dbContext.Eliminations
                                join pla_victim in _dbContext.Players on eli.PlayerVictimId equals pla_victim.Id
                                join usr_victim in _dbContext.Users on pla_victim.UserId equals usr_victim.Id
                                join pla_elim in _dbContext.Players on eli.PlayerEliminatorId equals pla_elim.Id
                                join usr_elim in _dbContext.Users on pla_elim.UserId equals usr_elim.Id
                                where allPlayerIds.Contains(eli.PlayerEliminatorId) || allPlayerIds.Contains(eli.PlayerVictimId)
                                select new EliminationResultDto
                                {
                                    Id = eli.Id,
                                    FirstNameEliminator = usr_elim.FirstName,
                                    LastNameEliminator = usr_elim.LastName,
                                    UserEliminatorId = usr_elim.Id,
                                    PlayerEliminatorId = pla_elim.Id,
                                    FirstNameVictim = usr_victim.FirstName,
                                    LastNameVictim = usr_victim.LastName,
                                    UserVictimId = usr_victim.Id,
                                    PlayerVictimId = pla_victim.Id
                                }
                            ).ToList();

            foreach (TournamentPlayedDto tournamentPlayedDto in results)
            {
                foreach (PlayerResultDto playerResultDto in tournamentPlayedDto.PlayerResults)
                {
                    playerResultDto.BonusTournamentEarneds = bonus.Where(b => b.PlayerId == playerResultDto.PlayerId).ToList();
                    playerResultDto.Eliminations = eliminations.Where(e => e.PlayerEliminatorId == playerResultDto.PlayerId).ToList();
                    playerResultDto.Victimisations = eliminations.Where(v => v.PlayerVictimId == playerResultDto.PlayerId).ToList();
                }
            }

            return results;
        }

        public Tournament? GetLastTournamentOver(bool includeOutOfSeason)
        {
            IEnumerable<Tournament> tournaments = _dbContext.Tournaments.Where(tou => (includeOutOfSeason || tou.Season != SeasonResources.OUT_OF_SEASON) && tou.IsOver);

            if (includeOutOfSeason)
            {
                return tournaments.OrderByDescending(tou => tou.StartDate)
                                  .FirstOrDefault();
            }
            else
            {
                return tournaments.OrderByDescending(tou => tou.Season)
                                  .ThenByDescending(tou => tou.StartDate)
                                  .FirstOrDefault();
            }
        }
    }
}
