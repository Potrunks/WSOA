using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Result
{
    public class MainNavMenuResult : APICallResult
    {
        public MainNavMenuResult() : base()
        {
            MainNavSectionVMs = new List<MainNavSectionViewModel>();
        }

        public MainNavMenuResult(string redirectUrl) : base(redirectUrl)
        {
            MainNavSectionVMs = new List<MainNavSectionViewModel>();
        }

        public MainNavMenuResult(string errorMsg, string redirectUrl) : base(errorMsg, redirectUrl)
        {
            MainNavSectionVMs = new List<MainNavSectionViewModel>();
        }

        public List<MainNavSectionViewModel> MainNavSectionVMs { get; set; }
    }
}
