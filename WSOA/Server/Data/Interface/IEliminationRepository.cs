using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IEliminationRepository
    {
        /// <summary>
        /// Save elimination.
        /// </summary>
        Elimination SaveElimination(Elimination elimination);

        /// <summary>
        /// Get all eliminations by player victim ids.
        /// </summary>
        IEnumerable<Elimination> GetEliminationsByPlayerVictimIds(IEnumerable<int> playerVictimIds);

        /// <summary>
        /// Get player victim qith all eliminations.
        /// </summary>
        IEnumerable<PlayerEliminationsDto> GetPlayerEliminationsDtosByPlayerVictimIds(IEnumerable<int> playerVictimIds);

        /// <summary>
        /// Delete elimination from DB.
        /// </summary>
        void DeleteElimination(Elimination elimination);

        /// <summary>
        /// Delete eliminations from DB.
        /// </summary>
        void DeleteEliminations(IEnumerable<Elimination> eliminations);
    }
}
