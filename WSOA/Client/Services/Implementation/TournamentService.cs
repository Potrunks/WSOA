using WSOA.Client.Services.Interface;
using WSOA.Client.Shared.Resources;
using WSOA.Client.Utils;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
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

        public async Task<APICallResultBase> SaveTournamentPrepared(TournamentPreparedDto tournamentPrepared)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(ApiRouteResources.PLAY_TOURNAMENT_PREPARED, tournamentPrepared.ToJsonUtf8());
            return response.Content.ToObject<APICallResultBase>();
        }

        public async Task<APICallResult<TournamentInProgressDto>> LoadTournamentInProgress(int subSectionId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(RouteResources.LOAD_TOURNAMENT_IN_PROGRESS, subSectionId));
            return response.Content.ToObject<APICallResult<TournamentInProgressDto>>();
        }

        public async Task<APICallResult<EliminationCreationResultDto>> EliminatePlayer(EliminationCreationDto elimination)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(RouteResources.ELIMINATE_PLAYER, elimination.ToJsonUtf8());
            return response.Content.ToObject<APICallResult<EliminationCreationResultDto>>();
        }

        public async Task<APICallResult<BonusTournamentEarnedEditResultDto>> SaveBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(RouteResources.SAVE_BONUS_EARNED, bonusTournamentEarnedEditDto.ToJsonUtf8());
            return response.Content.ToObject<APICallResult<BonusTournamentEarnedEditResultDto>>();
        }

        public async Task<APICallResult<BonusTournamentEarnedEditResultDto>> DeleteBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(RouteResources.DELETE_BONUS_EARNED, bonusTournamentEarnedEditDto.ToJsonUtf8());
            return response.Content.ToObject<APICallResult<BonusTournamentEarnedEditResultDto>>();
        }

        public async Task<APICallResult<CancelEliminationResultDto>> CancelLastPlayerEliminationByPlayerId(EliminationEditionDto eliminationEditionDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(RouteResources.CANCEL_PLAYER_ELIMINATION, eliminationEditionDto.ToJsonUtf8());
            return response.Content.ToObject<APICallResult<CancelEliminationResultDto>>();
        }

        public async Task<APICallResult<Player>> EditPlayerTotalAddon(int playerId, int addonNb)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(RouteResources.EDIT_ADDON_PLAYER, playerId, addonNb));
            return response.Content.ToObject<APICallResult<Player>>();
        }

        public async Task<APICallResultBase> RemovePlayerNeverComeIntoTournamentInProgress(int playerId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(RouteResources.REMOVE_PLAYER_NVR_COME, playerId));
            return response.Content.ToObject<APICallResultBase>();
        }

        public async Task<APICallResultBase> CancelTournamentInProgress(int tournamentInProgressId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(RouteResources.CANCEL_TOURNAMENT_IN_PROGRESS, tournamentInProgressId));
            return response.Content.ToObject<APICallResultBase>();
        }

        public async Task<APICallResult<IEnumerable<PlayerPlayingDto>>> AddPlayersIntoTournamentInProgress(IEnumerable<int> usrIds, int tournamentId)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(string.Format(RouteResources.ADD_PLAYERS_TOURNAMENT_IN_PROGRESS, tournamentId), usrIds.ToJsonUtf8());
            return response.Content.ToObject<APICallResult<IEnumerable<PlayerPlayingDto>>>();
        }

        public async Task<APICallResult<PlayerSelectionViewModel>> LoadPlayersForPlayingTournamentInProgress(int tournamentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format(ApiRouteResources.ADD_PLAYERS_TOURNAMENT_IN_PROGRESS, tournamentId));
            return response.Content.ToObject<APICallResult<PlayerSelectionViewModel>>();
        }

        public async Task<APICallResult<TournamentStepEnum>> GoToTournamentInProgressNextStep(int tournamentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format("api/tournament/inProgress/{0}/nextStep", tournamentId));
            return response.Content.ToObject<APICallResult<TournamentStepEnum>>();
        }

        public async Task<APICallResult<TournamentStepEnum>> GoToTournamentInProgressPreviousStep(int tournamentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format("api/tournament/inProgress/{0}/previousStep", tournamentId));
            return response.Content.ToObject<APICallResult<TournamentStepEnum>>();
        }

        public async Task<APICallResultBase> DeletePlayableTournament(int tournamentToDeleteId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(string.Format("api/tournament/playable/{0}/delete", tournamentToDeleteId));
            return response.Content.ToObject<APICallResultBase>();
        }
    }
}
