using WSOA.Server.Data.Interface;
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
                where pla.PlayedTournamentId == tournamentId && pla.UserId == userId
                select pla
            )
            .SingleOrDefault();
        }

        public void SavePlayer(Player player)
        {
            if (player.Id == 0)
            {
                _dbContext.Players.Add(player);
            }
            _dbContext.SaveChanges();
        }
    }
}
