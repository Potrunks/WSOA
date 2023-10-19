using Microsoft.AspNetCore.Mvc;
using WSOA.Server.Business.Interface;
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
    }
}
