using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class MainNavMenuViewModel
    {
        public MainNavMenuViewModel()
        {

        }

        public MainNavMenuViewModel(IDictionary<MainNavSection, List<MainNavSubSection>> subSectionsBySection)
        {
            MainNavSectionVMs = subSectionsBySection.Select(kvp => new MainNavSectionViewModel(kvp))
                                                    .ToList();
        }

        public List<MainNavSectionViewModel> MainNavSectionVMs { get; set; }
    }
}
