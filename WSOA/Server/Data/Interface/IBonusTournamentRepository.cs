using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IBonusTournamentRepository
    {
        /// <summary>
        /// Get all bonus tournaments.
        /// </summary>
        IEnumerable<BonusTournament> GetAll();

        /// <summary>
        /// Get bonus tournament by code.
        /// </summary>
        BonusTournament GetBonusTournamentByCode(string code);

        /// <summary>
        /// Get bonus tournaments list by list of bonus tournament code.
        /// </summary>
        IEnumerable<BonusTournament> GetBonusTournamentsByCodes(IEnumerable<string> codes);
    }
}
