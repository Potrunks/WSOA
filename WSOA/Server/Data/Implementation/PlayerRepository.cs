﻿using WSOA.Server.Data.Interface;
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
    }
}
