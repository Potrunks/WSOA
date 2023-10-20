using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Resources;
using WSOA.Client.Utils;
using WSOA.Shared.Dtos;
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

        public async Task<APICallResult<TournamentsViewModel>> LoadTournamentsNotOver(int subSectionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.LOAD_TOURNAMENTS_NOT_OVER, subSectionId));
            return response.Content.ToObject<APICallResult<TournamentsViewModel>>();
        }

        public async Task<APICallResult<PlayerViewModel>> SignUpTournament(SignUpTournamentFormViewModel form)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(ApiRouteResources.SIGN_UP_TOURNAMENT, form.ToJsonUtf8());
            return response.Content.ToObject<APICallResult<PlayerViewModel>>();
        }

        public async Task<APICallResult<PlayerSelectionViewModel>> LoadPlayersForPlayingTournament(int tournamentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.TOURNAMENT_PREPARATION_GET_PLAYERS, tournamentId));
            return response.Content.ToObject<APICallResult<PlayerSelectionViewModel>>();
        }

        public async Task<APICallResultBase> PlayTournamentPrepared(TournamentPreparedDto tournamentPrepared)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(ApiRouteResources.PLAY_TOURNAMENT_PREPARED, tournamentPrepared.ToJsonUtf8());
            return response.Content.ToObject<APICallResultBase>();
        }
    }
}
