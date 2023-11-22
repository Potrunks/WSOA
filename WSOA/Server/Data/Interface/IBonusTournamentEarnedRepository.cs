using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IBonusTournamentEarnedRepository
    {
        /// <summary>
        /// Save bonus tournament earned.
        /// </summary>
        BonusTournamentEarned SaveBonusTournamentEarned(BonusTournamentEarned bonusTournamentEarned);
    }
}
