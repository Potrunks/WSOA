using WSOA.Shared.ViewModel;

namespace WSOA.Shared.Result
{
    public class MainNavMenuResult : APICallResult
    {
        public MainNavMenuResult(List<MainNavSectionViewModel> mainNavSectionVMs) : base()
        {
            MainNavSectionVMs = mainNavSectionVMs;
        }

        public List<MainNavSectionViewModel> MainNavSectionVMs { get; set; }
    }
}
