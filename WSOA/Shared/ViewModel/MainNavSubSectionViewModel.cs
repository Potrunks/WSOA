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
            Id = 0;
        }

        public MainNavSubSectionViewModel(MainNavSubSection mainNavSubSection)
        {
            Label = mainNavSubSection.Label;
            Order = mainNavSubSection.Order;
            Url = mainNavSubSection.Url + "/" + mainNavSubSection.Id;
            Id = mainNavSubSection.Id;
        }

        public string Label { get; set; }

        public int Order { get; set; }

        public string Url { get; set; }

        public int Id { get; set; }
    }
}
