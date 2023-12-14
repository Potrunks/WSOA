using Microsoft.AspNetCore.Mvc;
using WSOA.Server.Business.Interface;
using WSOA.Shared.Dtos;
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
        /// Save and play the tournament selected.
        /// </summary>
        [HttpPost]
        [Route("api/tournament/prepared/save")]
        public APICallResultBase SaveTournamentPrepared([FromBody] TournamentPreparedDto tournamentPrepared)
        {
            return _tournamentBusiness.SaveTournamentPrepared(tournamentPrepared, HttpContext.Session);
        }

        [HttpGet]
        [Route("api/tournament/inProgress/load/{subSectionId}")]
        public APICallResult<TournamentInProgressDto> LoadTournamentInProgress(int subSectionId)
        {
            return _tournamentBusiness.LoadTournamentInProgress(subSectionId, HttpContext.Session);
        }

        [HttpPost]
        [Route("api/tournament/eliminatePlayer")]
        public APICallResult<EliminationCreationResultDto> EliminatePlayer(EliminationCreationDto elimination)
        {
            return _tournamentBusiness.EliminatePlayer(elimination, HttpContext.Session);
        }

        [HttpPost]
        [Route("api/tournament/saveBonusEarned")]
        public APICallResult<BonusTournamentEarnedEditResultDto> SaveBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedCreation)
        {
            return _tournamentBusiness.SaveBonusTournamentEarned(bonusTournamentEarnedCreation, HttpContext.Session);
        }

        [HttpPost]
        [Route(RouteResources.DELETE_BONUS_EARNED)]
        public APICallResult<BonusTournamentEarnedEditResultDto> DeleteBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto)
        {
            return _tournamentBusiness.DeleteBonusTournamentEarned(bonusTournamentEarnedEditDto, HttpContext.Session);
        }

        [HttpGet]
        [Route("api/tournament/cancel/player/{playerId}/elimination")]
        public APICallResult<CancelEliminationResultDto> CancelLastPlayerEliminationByPlayerId(int playerId)
        {
            return _tournamentBusiness.CancelLastPlayerElimination(playerId, HttpContext.Session);
        }
    }
}
