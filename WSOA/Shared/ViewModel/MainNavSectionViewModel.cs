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

        public MainNavSectionViewModel(KeyValuePair<MainNavSection, List<MainNavSubSection>> subSectionsBySection)
        {
            ClassIcon = subSectionsBySection.Key.ClassIcon;
            Order = subSectionsBySection.Key.Order;
            MainNavSubSectionVMs = subSectionsBySection.Value.Select(ss => new MainNavSubSectionViewModel(ss)).ToList();
        }

        public string ClassIcon { get; set; }

        public int Order { get; set; }

        public List<MainNavSubSectionViewModel> MainNavSubSectionVMs { get; set; }
    }
}
