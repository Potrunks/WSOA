using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Result
{
    public class SignUpTournamentCallResult : APICallResult
    {
        public SignUpTournamentCallResult() : base()
        {

        }

        public SignUpTournamentCallResult(string? redirectUrl) : base(redirectUrl)
        {

        }

        public SignUpTournamentCallResult(string errorMsg, string? redirectUrl) : base(errorMsg, redirectUrl)
        {

        }

        public PlayerDataViewModel PlayerSignedUp { get; set; }
    }
}
