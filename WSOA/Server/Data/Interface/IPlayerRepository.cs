using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IPlayerRepository
    {
        /// <summary>
        /// Get player by tournament ID and User ID.
        /// </summary>
        Player? GetPlayerByTournamentIdAndUserId(int tournamentId, int userId);

        /// <summary>
        /// Save player.
        /// </summary>
        void SavePlayer(Player player);

        /// <summary>
        /// Save Players.
        /// </summary>
        void SavePlayers(IEnumerable<Player> players);

        /// <summary>
        /// Get players into tournament and by presence state code.
        /// </summary>
        IEnumerable<PlayerDto> GetPlayersByTournamentIdAndPresenceStateCode(int tournamentId, string presenceStateCode);

        /// <summary>
        /// Delete players.
        /// </summary>
        void DeletePlayers(IEnumerable<Player> players);

        /// <summary>
        /// Get bonus tournament earned list by player id.
        /// </summary>
        IDictionary<int, IEnumerable<BonusTournamentEarned>> GetBonusTournamentEarnedsByPlayerIds(IEnumerable<int> playerIds);

        /// <summary>
        /// Get players by ids.
        /// </summary>
        IDictionary<int, Player> GetPlayersByIds(IEnumerable<int> playerIds);

        /// <summary>
        /// Get player dtos by player Ids.
        /// </summary>
        IEnumerable<PlayerDto> GetPlayerDtosByPlayerIds(IEnumerable<int> playerIds);
    }
}
