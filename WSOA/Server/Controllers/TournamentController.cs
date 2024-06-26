﻿using Microsoft.AspNetCore.Mvc;
using WSOA.Server.Business.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Controllers
{
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private ITournamentBusiness _tournamentBusiness;

        public TournamentController
        (
            ITournamentBusiness tournamentBusiness
        )
        {
            _tournamentBusiness = tournamentBusiness;
        }

        /// <summary>
        /// Create tournament.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/create")]
        public APICallResultBase CreateTournament([FromBody] TournamentCreationFormViewModel form)
        {
            return _tournamentBusiness.CreateTournament(form, HttpContext.Session);
        }

        /// <summary>
        /// Load datas for tournament creation.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/create/load/{subSectionId}")]
        public APICallResult<TournamentCreationDataViewModel> LoadTournamentCreationDatas(int subSectionId)
        {
            return _tournamentBusiness.LoadTournamentCreationDatas(subSectionId, HttpContext.Session);
        }

        /// <summary>
        /// Load future tournament datas.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/future/load/{subSectionId}")]
        public APICallResult<TournamentsViewModel> LoadTournamentsNotOver(int subSectionId)
        {
            return _tournamentBusiness.LoadTournamentsNotOver(subSectionId, HttpContext.Session);
        }

        /// <summary>
        /// Sign up current user into the tournament
        /// </summary>
        [HttpPost]
        [Route("api/tournament/future/signUp")]
        public APICallResult<PlayerViewModel> SignUpTournament([FromBody] SignUpTournamentFormViewModel form)
        {
            return _tournamentBusiness.SignUpTournament(form, HttpContext.Session);
        }

        /// <summary>
        /// Get present players and possible players for tournament in preparation
        /// </summary>
        [HttpGet]
        [Route("api/tournament/load/presentPlayers/{tournamentId}")]
        public APICallResult<PlayerSelectionViewModel> LoadPlayersForPlayingTournament(int tournamentId)
        {
            return _tournamentBusiness.LoadPlayersForPlayingTournament(tournamentId, HttpContext.Session);
        }

        /// <summary>
        /// Load users can be add into tournament in progress.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/inProgress/{tournamentId}/load/newPlayers")]
        public APICallResult<PlayerSelectionViewModel> LoadPlayersForPlayingTournamentInProgress(int tournamentId)
        {
            return _tournamentBusiness.LoadPlayersForPlayingTournamentInProgress(tournamentId, HttpContext.Session);
        }

        /// <summary>
        /// Save and play the tournament selected.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/prepared/save")]
        public APICallResultBase SaveTournamentPrepared([FromBody] TournamentPreparedDto tournamentPrepared)
        {
            return _tournamentBusiness.SaveTournamentPrepared(tournamentPrepared, HttpContext.Session);
        }

        /// <summary>
        /// Load the tournament in progress.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/inProgress/load/{subSectionId}")]
        public APICallResult<TournamentInProgressDto> LoadTournamentInProgress(int subSectionId)
        {
            return _tournamentBusiness.LoadTournamentInProgress(subSectionId, HttpContext.Session);
        }

        /// <summary>
        /// Eliminate the selected player.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/eliminatePlayer")]
        public APICallResult<EliminationCreationResultDto> EliminatePlayer(EliminationCreationDto elimination)
        {
            return _tournamentBusiness.EliminatePlayer(elimination, HttpContext.Session);
        }

        /// <summary>
        /// Save the bonus tournament selected for the player selected.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/saveBonusEarned")]
        public APICallResult<BonusTournamentEarnedEditResultDto> SaveBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedCreation)
        {
            return _tournamentBusiness.SaveBonusTournamentEarned(bonusTournamentEarnedCreation, HttpContext.Session);
        }

        /// <summary>
        /// Delete the bonus tournament earned selected for the player selected.
        /// </summary>
        [HttpPost]
        [Route(RouteResources.DELETE_BONUS_EARNED)]
        public APICallResult<BonusTournamentEarnedEditResultDto> DeleteBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto)
        {
            return _tournamentBusiness.DeleteBonusTournamentEarned(bonusTournamentEarnedEditDto, HttpContext.Session);
        }

        /// <summary>
        /// Cancel the lest elimination for the selected player.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/cancel/player/elimination")]
        public APICallResult<CancelEliminationResultDto> CancelLastPlayerEliminationByPlayerId([FromBody] EliminationEditionDto eliminationEditionDto)
        {
            return _tournamentBusiness.CancelLastPlayerElimination(eliminationEditionDto, HttpContext.Session);
        }

        /// <summary>
        /// Edit the value of the total addon of the selected player.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/player/{playerId}/addon/{addonNb}")]
        public APICallResult<PlayerAddonEditionResultDto> EditPlayerTotalAddon(int playerId, int addonNb)
        {
            return _tournamentBusiness.EditPlayerTotalAddon(playerId, addonNb, HttpContext.Session);
        }

        /// <summary>
        /// Remove player never come into tournament in progress.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/player/{playerId}/neverCome")]
        public APICallResult<IEnumerable<JackpotDistribution>> RemovePlayerNeverComeIntoTournamentInProgress(int playerId)
        {
            return _tournamentBusiness.RemovePlayerNeverComeIntoTournamentInProgress(playerId, HttpContext.Session);
        }

        /// <summary>
        /// Cancel tournament in progress.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/inProgress/{tournamentInProgressId}/cancel")]
        public APICallResultBase CancelTournamentInProgress(int tournamentInProgressId)
        {
            return _tournamentBusiness.CancelTournamentInProgress(tournamentInProgressId, HttpContext.Session);
        }

        /// <summary>
        /// Add players into tournament in progress.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/inProgress/{tournamentId}/addPlayers")]
        public APICallResult<AddPlayersResultDto> AddPlayersIntoTournamentInProgress([FromBody] IEnumerable<int> usrIds, int tournamentId)
        {
            return _tournamentBusiness.AddPlayersIntoTournamentInProgress(usrIds, tournamentId, HttpContext.Session);
        }

        /// <summary>
        /// Go to next step for tournament in progress.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/inProgress/{tournamentId}/nextStep")]
        public APICallResult<TournamentStepEnum> GoToTournamentInProgressNextStep(int tournamentId)
        {
            return _tournamentBusiness.GoToTournamentInProgressNextStep(tournamentId, HttpContext.Session);
        }

        /// <summary>
        /// Go to previous step for tournament in progress.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/inProgress/{tournamentId}/previousStep")]
        public APICallResult<SwitchTournamentStepResultDto> GoToTournamentInProgressPreviousStep(int tournamentId)
        {
            return _tournamentBusiness.GoToTournamentInProgressPreviousStep(tournamentId, HttpContext.Session);
        }

        /// <summary>
        /// Delete playable tournament selected.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/playable/{tournamentIdToDelete}/delete")]
        public APICallResultBase DeletePlayableTournament(int tournamentIdToDelete)
        {
            return _tournamentBusiness.DeletePlayableTournament(tournamentIdToDelete, HttpContext.Session);
        }

        /// <summary>
        /// Load season result by season selected.
        /// </summary>
        [HttpGet]
        [Route("api/season/{season}/result")]
        public APICallResult<SeasonResultDto> LoadSeasonResult(int season)
        {
            return _tournamentBusiness.LoadSeasonResultDto(season, HttpContext.Session);
        }

        /// <summary>
        /// Edit winnable moneys by position during a tournament in progress.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/inProgress/{tournamentId}/editWinnableMoneys")]
        public APICallResult<IEnumerable<JackpotDistribution>> EditWinnableMoneysByPosition([FromBody] IDictionary<int, int> winnableMoneysByPosition, int tournamentId)
        {
            return _tournamentBusiness.EditWinnableMoneysByPosition(winnableMoneysByPosition, tournamentId, HttpContext.Session);
        }

        /// <summary>
        /// Load the season in progress result of the current user connected.
        /// </summary>
        [HttpGet]
        [Route("api/season/in-progress/my/load")]
        public APICallResult<SeasonMyResultDto> LoadMySeasonInProgressResultDto()
        {
            return _tournamentBusiness.LoadMySeasonInProgressResultDto(HttpContext.Session);
        }
    }
}
