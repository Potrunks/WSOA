using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Interface
{
    public interface ITournamentService
    {
        /// <summary>
        /// Create tournament.
        /// </summary>
        Task<APICallResultBase> CreateTournament(TournamentCreationFormViewModel form);

        /// <summary>
        /// Load datas for tournament creation.
        /// </summary>
        Task<APICallResult<TournamentCreationDataViewModel>> LoadTournamentCreationDatas(int subSectionId);

        /// <summary>
        /// Load tournaments not over.
        /// </summary>
        Task<APICallResult<TournamentsViewModel>> LoadTournamentsNotOver(int subSectionId);

        /// <summary>
        /// Sign up tournament.
        /// </summary>
        Task<APICallResult<PlayerViewModel>> SignUpTournament(SignUpTournamentFormViewModel form);

        /// <summary>
        /// Load present players and available players for tournament preparation.
        /// </summary>
        Task<APICallResult<PlayerSelectionViewModel>> LoadPlayersForPlayingTournament(int tournamentId);

        /// <summary>
        /// Save selected tournament and execute it.
        /// </summary>
        Task<APICallResultBase> SaveTournamentPrepared(TournamentPreparedDto tournamentPrepared);

        /// <summary>
        /// Load tournament in progress.
        /// </summary>
        Task<APICallResult<TournamentInProgressDto>> LoadTournamentInProgress(int subSectionId);

        /// <summary>
        /// Eliminate selected player and close tournament if it's the final.
        /// </summary>
        Task<APICallResult<EliminationCreationResultDto>> EliminatePlayer(EliminationCreationDto elimination);

        /// <summary>
        /// Save bonus tournament earned by a player
        /// </summary>
        Task<APICallResult<BonusTournamentEarnedEditResultDto>> SaveBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto);

        /// <summary>
        /// Delete bonus tournament earned by a player (reduce by one the occurence or delete if occurence updated is zero).
        /// </summary>
        Task<APICallResult<BonusTournamentEarnedEditResultDto>> DeleteBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto);

        /// <summary>
        /// Cancel the last player elimination.
        /// </summary>
        Task<APICallResult<CancelEliminationResultDto>> CancelLastPlayerEliminationByPlayerId(EliminationEditionDto eliminationEditionDto);

        /// <summary>
        /// Edit the value of the total addon of the selected player.
        /// </summary>
        Task<APICallResult<Player>> EditPlayerTotalAddon(int playerId, int addonNb);

        /// <summary>
        /// Remove player never come into tournament in progress.
        /// </summary>
        Task<APICallResultBase> RemovePlayerNeverComeIntoTournamentInProgress(int playerId);

        /// <summary>
        /// Cancel tournament in progress.
        /// </summary>
        Task<APICallResultBase> CancelTournamentInProgress(int tournamentInProgressId);

        /// <summary>
        /// Add players into tournament in progress.
        /// </summary>
        Task<APICallResult<IEnumerable<PlayerPlayingDto>>> AddPlayersIntoTournamentInProgress(IEnumerable<int> usrIds, int tournamentId);

        /// <summary>
        /// Load users can be add into tournament in progress.
        /// </summary>
        Task<APICallResult<PlayerSelectionViewModel>> LoadPlayersForPlayingTournamentInProgress(int tournamentId);

        /// <summary>
        /// Go to next step for tournament in progress.
        /// </summary>
        Task<APICallResult<TournamentStepEnum>> GoToTournamentInProgressNextStep(int tournamentId);

        /// <summary>
        /// Go to previous step for tournament in progress.
        /// </summary>
        Task<APICallResult<TournamentStepEnum>> GoToTournamentInProgressPreviousStep(int tournamentId);

        /// <summary>
        /// Delete playable tournament by selected ID.
        /// </summary>
        Task<APICallResultBase> DeletePlayableTournament(int tournamentToDeleteId);

        /// <summary>
        /// Load season result by selected season.
        /// </summary>
        Task<APICallResult<SeasonResultViewModel>> LoadSeasonResult(int seasonSelected);
    }
}
