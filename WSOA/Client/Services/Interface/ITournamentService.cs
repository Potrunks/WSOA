using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Interface
{
    public interface ITournamentService
    {
        /// <summary>
        /// Create tournament.
        /// </summary>
        Task<APICallResult> CreateTournament(TournamentCreationFormViewModel form);

        /// <summary>
        /// Load datas for tournament creation.
        /// </summary>
        Task<CreateTournamentCallResult> LoadTournamentCreationDatas(int subSectionId);

        /// <summary>
        /// Load future tournament datas.
        /// </summary>
        Task<LoadFutureTournamentCallResult> LoadFutureTournamentDatas(int subSectionId);
    }
}
