using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Resources;
using WSOA.Client.Utils;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Services.Implementation
{
    public class TournamentService : ITournamentService
    {
        private readonly HttpClient _httpClient;

        public TournamentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APICallResultBase> CreateTournament(TournamentCreationFormViewModel form)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(ApiRouteResources.CREATE_TOURNAMENT, form.ToJsonUtf8());
            return response.Content.ToObject<APICallResultBase>();
        }

        public async Task<APICallResult<TournamentCreationDataViewModel>> LoadTournamentCreationDatas(int subSectionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.LOAD_CREATE_TOURNAMENT_DATAS, subSectionId));
            return response.Content.ToObject<APICallResult<TournamentCreationDataViewModel>>();
        }

        public async Task<APICallResult<TournamentsViewModel>> LoadFutureTournamentDatas(int subSectionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.LOAD_FUTURE_TOURNAMENT_DATAS, subSectionId));
            return response.Content.ToObject<APICallResult<TournamentsViewModel>>();
        }

        public async Task<APICallResult<PlayerDataViewModel>> SignUpTournament(SignUpTournamentFormViewModel form)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(ApiRouteResources.SIGN_UP_TOURNAMENT, form.ToJsonUtf8());
            return response.Content.ToObject<APICallResult<PlayerDataViewModel>>();
        }

        public async Task<APICallResult<TournamentsViewModel>> LoadPlayableTournaments(int subSectionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.LOAD_PLAYABLE_TOURNAMENT_DATAS, subSectionId));
            return response.Content.ToObject<APICallResult<TournamentsViewModel>>();
        }
    }
}
