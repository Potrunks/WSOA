using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface ITournamentRepository
    {
        /// <summary>
        /// Save tournament in DB.
        /// </summary>
        void SaveTournament(Tournament tournament);
    }
}
