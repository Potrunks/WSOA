using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Result
{
    public class TournamentCallResult : APICallResult
    {
        public TournamentCallResult() : base(null)
        {

        }

        public TournamentsViewModel Data { get; set; }
    }
}
