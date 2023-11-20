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
    }
}
