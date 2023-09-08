using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Result
{
    public class LoadFutureTournamentCallResult : APICallResult
    {
        public LoadFutureTournamentCallResult() : base(null)
        {

        }

        public LoadFutureTournamentCallResult(string errorMsg, string? redirectUrl) : base(errorMsg, redirectUrl)
        {

        }

        public List<FutureTournamentDataViewModel> Datas { get; set; }
    }
}
