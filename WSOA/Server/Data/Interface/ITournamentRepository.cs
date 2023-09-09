using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;

namespace WSOA.Server.Data.Interface
{
    public interface ITournamentRepository
    {
        /// <summary>
        /// Save tournament in DB.
        /// </summary>
        void SaveTournament(Tournament tournament);

        /// <summary>
        /// Get tournaments and players attached by is over value.
        /// </summary>
        List<TournamentDto> GetTournamentDtosByIsOver(bool isOver);
    }
}
