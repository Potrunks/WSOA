using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class MainNavSubSectionViewModel
    {
        public MainNavSubSectionViewModel()
        {
            Label = "Error";
            Order = 99;
            Url = null;
        }

        public MainNavSubSectionViewModel(MainNavSubSection mainNavSubSection)
        {
            Label = mainNavSubSection.Label;
            Order = mainNavSubSection.Order;
            Url = mainNavSubSection.Url;
        }

        public string Label { get; set; }

        public int Order { get; set; }

        public string Url { get; set; }
    }
}
