using WSOA.Shared.Dtos;
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
        Task<APICallResult<CancelEliminationResultDto>> CancelLastPlayerEliminationByPlayerId(int playerId);
    }
}
