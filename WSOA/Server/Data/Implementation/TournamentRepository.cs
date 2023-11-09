using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
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

        public Tournament GetPreviousTournament(Tournament currentTournament)
        {
            List<Tournament> tournaments = new List<Tournament> { currentTournament };
            tournaments.AddRange(_dbContext.Tournaments.Where(tou => tou.Season == currentTournament.Season && tou.IsOver));
            List<Tournament> orderedTournaments = tournaments.OrderBy(tou => tou.StartDate).ToList();

            int indexOfCurrentTournament = orderedTournaments.IndexOf(currentTournament);

            if (tournaments.Count == 1)
            {
                return currentTournament;
            }
            else
            {
                return orderedTournaments[indexOfCurrentTournament - 1];
            }
        }
    }
}
