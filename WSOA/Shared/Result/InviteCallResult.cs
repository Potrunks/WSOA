using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Result
{
    public class InviteCallResult : APICallResult
    {
        public InviteCallResult() : base()
        {
            InviteVM = new InviteViewModel();
        }

        public InviteCallResult(string errorMsg, string redirectUrl) : base(errorMsg, redirectUrl)
        {
            InviteVM = new InviteViewModel();
        }

        public InviteViewModel InviteVM { get; set; }
    }
}
