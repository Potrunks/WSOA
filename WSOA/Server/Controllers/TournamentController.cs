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
        public APICallResult CreateTournament([FromBody] TournamentCreationFormViewModel form)
        {
            return _tournamentBusiness.CreateTournament(form, HttpContext.Session);
        }
    }
}
