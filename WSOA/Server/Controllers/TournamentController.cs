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
        public APICallResult<TournamentsViewModel> LoadFutureTournamentDatas(int subSectionId)
        {
            return _tournamentBusiness.LoadFutureTournamentDatas(subSectionId, HttpContext.Session);
        }

        /// <summary>
        /// Sign up current user into the tournament
        /// </summary>
        [HttpPost]
        [Route("api/tournament/future/signUp")]
        public APICallResult<PlayerDataViewModel> SignUpTournament([FromBody] SignUpTournamentFormViewModel form)
        {
            return _tournamentBusiness.SignUpTournament(form, HttpContext.Session);
        }

        /// <summary>
        /// Load playable tournaments.
        /// </summary>
        [HttpGet]
        [Route("api/tournament/playable/load/{subSectionId}")]
        public APICallResult<TournamentsViewModel> LoadPlayableTournaments(int subSectionId)
        {
            return _tournamentBusiness.LoadPlayableTournaments(subSectionId, HttpContext.Session);
        }
    }
}
