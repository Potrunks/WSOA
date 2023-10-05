using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Interface
{
    public interface ITournamentBusiness
    {
        /// <summary>
        /// Create new tournament and prevent all users in app.
        /// </summary>
        APICallResultBase CreateTournament(TournamentCreationFormViewModel form, ISession session);

        /// <summary>
        /// Load datas for tournament creation.
        /// </summary>
        APICallResult<TournamentCreationDataViewModel> LoadTournamentCreationDatas(int subSectionId, ISession session);

        /// <summary>
        /// Load future tournament datas.
        /// </summary>
        APICallResult<FutureTournamentsViewModel> LoadFutureTournamentDatas(int subSectionId, ISession session);

        /// <summary>
        /// Sign up the current user to the tournament selected.
        /// </summary>
        APICallResult<PlayerDataViewModel> SignUpTournament(SignUpTournamentFormViewModel formVM, ISession session);

        /// <summary>
        /// Load all playable tournaments.
        /// </summary>
        APICallResult<PlayableTournamentsViewModel> LoadPlayableTournaments(int subSectionId, ISession session);
    }
}
