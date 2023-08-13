using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class MainNavSectionViewModel
    {
        public MainNavSectionViewModel()
        {
            MainNavSubSectionVMs = new List<MainNavSubSectionViewModel>();
            ClassIcon = "";
            Order = 99;
        }

        public MainNavSectionViewModel(MainNavSection mainNavSection)
        {
            ClassIcon = mainNavSection.ClassIcon;
            Order = mainNavSection.Order;
            MainNavSubSectionVMs = new List<MainNavSubSectionViewModel>();
        }

        public string ClassIcon { get; set; }

        public int Order { get; set; }

        public List<MainNavSubSectionViewModel> MainNavSubSectionVMs { get; set; }
    }
}
