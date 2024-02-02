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
        List<TournamentDto> GetTournamentDtosByIsOverAndIsInProgress(bool isOver, bool isInProgress);

        /// <summary>
        /// Get tournament by ID.
        /// </summary>
        Tournament GetTournamentById(int tournamentId);

        /// <summary>
        /// Check if tournament exists by ID and is over state.
        /// </summary>
        bool ExistsTournamentByTournamentIdIsOverAndIsInProgress(int tournamentId, bool isOver, bool isInProgress);

        /// <summary>
        /// Check if exists tournament by is in progress status.
        /// </summary>
        bool ExistsTournamentByIsInProgress(bool isInProgress);

        /// <summary>
        /// Get tournament DTO by ID.
        /// </summary>
        TournamentDto GetTournamentDtoById(int tournamentId);

        /// <summary>
        /// Get tournament in progress.
        /// </summary>
        Tournament? GetTournamentInProgress();

        /// <summary>
        /// Get the tournament number into the season given.
        /// </summary>
        int GetTournamentNumber(Tournament tournament);

        /// <summary>
        /// Get the previous tournament into the same season of the tournament given.
        /// </summary>
        Tournament GetPreviousTournament(Tournament currentTournament);

        /// <summary>
        /// Get the last tournament over by season code given.
        /// </summary>
        Tournament? GetLastFinishedTournamentBySeason(string season);

        /// <summary>
        /// Get all data to cancel a tournament.
        /// </summary>
        TournamentToCancelDto GetTournamentToCancelDtoByTournamentId(int tournamentToCancelId);
    }
}
