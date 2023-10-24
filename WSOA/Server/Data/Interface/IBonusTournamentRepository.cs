using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IBonusTournamentRepository
    {
        /// <summary>
        /// Get all bonus tournaments.
        /// </summary>
        IEnumerable<BonusTournament> GetAll();
    }
}
