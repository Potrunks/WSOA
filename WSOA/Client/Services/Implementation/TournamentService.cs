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

        public async Task<APICallResult> CreateTournament(TournamentCreationFormViewModel form)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(ApiRouteResources.CREATE_TOURNAMENT, form.ToJsonUtf8());
            return response.Content.ToObject<APICallResult>();
        }

        public async Task<CreateTournamentCallResult> LoadTournamentCreationDatas(int subSectionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.LOAD_CREATE_TOURNAMENT_DATAS, subSectionId));
            return response.Content.ToObject<CreateTournamentCallResult>();
        }

        public async Task<LoadFutureTournamentCallResult> LoadFutureTournamentDatas(int subSectionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.LOAD_FUTURE_TOURNAMENT_DATAS, subSectionId));
            return response.Content.ToObject<LoadFutureTournamentCallResult>();
        }
    }
}
