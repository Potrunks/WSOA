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

        public List<TournamentDto> GetTournamentDtosByIsOver(bool isOver)
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
    }
}
