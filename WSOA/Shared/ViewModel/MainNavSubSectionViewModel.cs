using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class MainNavSubSectionViewModel
    {
        public MainNavSubSectionViewModel()
        {
            Label = "";
            Order = 99;
        }

        public MainNavSubSectionViewModel(MainNavSubSection mainNavSubSection)
        {
            Label = mainNavSubSection.Label;
            Order = mainNavSubSection.Order;
        }

        public string Label { get; set; }

        public int Order { get; set; }
    }
}
