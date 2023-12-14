using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IBonusTournamentEarnedRepository
    {
        /// <summary>
        /// Save bonus tournament earned.
        /// </summary>
        BonusTournamentEarned SaveBonusTournamentEarned(BonusTournamentEarned bonusTournamentEarned);

        /// <summary>
        /// Delete bonus tournament earned. If Occurence more than 1, just reduce by one the occurence and save, else remove from DB.
        /// </summary>
        BonusTournamentEarned DeleteBonusTournamentEarned(int playerIdConcerned, string bonusTournamentCodeToDelete);

        /// <summary>
        /// Delete many bonus tournament earned.
        /// </summary>
        void DeleteBonusTournamentEarneds(IEnumerable<BonusTournamentEarned> bonusTournamentEarneds);
    }
}
