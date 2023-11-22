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
        Task<APICallResult<EliminationResultDto>> EliminatePlayer(EliminationDto elimination);
    }
}
