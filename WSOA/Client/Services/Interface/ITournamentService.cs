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
    }
}
