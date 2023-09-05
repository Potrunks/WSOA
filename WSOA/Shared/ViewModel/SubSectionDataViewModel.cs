namespace WSOA.Shared.ViewModel
{
    public class SubSectionDataViewModel
    {
        public SubSectionDataViewModel()
        {

        }

        public SubSectionDataViewModel(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
