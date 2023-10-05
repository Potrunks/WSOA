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
        /// Load future tournament datas.
        /// </summary>
        Task<APICallResult<FutureTournamentsViewModel>> LoadFutureTournamentDatas(int subSectionId);

        /// <summary>
        /// Sign up tournament.
        /// </summary>
        Task<APICallResult<PlayerDataViewModel>> SignUpTournament(SignUpTournamentFormViewModel form);

        /// <summary>
        /// Load playable tournaments.
        /// </summary>
        Task<APICallResult<PlayableTournamentsViewModel>> LoadPlayableTournaments(int subSectionId);
    }
}
