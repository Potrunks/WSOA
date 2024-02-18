using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface IJackpotDistributionRepository
    {
        /// <summary>
        /// Save jackpot distributions.
        /// </summary>
        void SaveJackpotDistributions(IEnumerable<JackpotDistribution> jackpotDistributions);

        /// <summary>
        /// Get jackpot distributions by tournament Id.
        /// </summary>
        List<JackpotDistribution> GetJackpotDistributionsByTournamentId(int tournamentId);

        /// <summary>
        /// Remove jackpot distributions.
        /// </summary>
        void RemoveJackpotDistributions(IEnumerable<JackpotDistribution> jackpotDistributions);
    }
}
