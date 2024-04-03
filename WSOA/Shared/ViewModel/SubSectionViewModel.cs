namespace WSOA.Shared.ViewModel
{
    public class SubSectionViewModel
    {
        public SubSectionViewModel()
        {

        }

        public SubSectionViewModel(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
