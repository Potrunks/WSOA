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
        /// Get players into tournament and by presence state code.
        /// </summary>
        IEnumerable<PlayerDto> GetPlayersByTournamentIdAndPresenceStateCode(int tournamentId, string presenceStateCode);
    }
}
