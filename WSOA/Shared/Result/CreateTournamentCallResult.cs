using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Result
{
    public class CreateTournamentCallResult : APICallResult
    {
        public CreateTournamentCallResult()
        {

        }

        public CreateTournamentCallResult(string? redirectUrl) : base(redirectUrl)
        {

        }

        public CreateTournamentCallResult(string errorMsg, string? redirectUrl) : base(errorMsg, redirectUrl)
        {

        }

        public TournamentCreationDataViewModel? Data { get; set; }
    }
}
