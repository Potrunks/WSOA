namespace WSOA.Shared.ViewModel
{
    public class InviteViewModel : SubSectionViewModel
    {
        public InviteViewModel()
        {
            ProfileLabelsByCode = new Dictionary<string, string>();
        }

        public InviteViewModel(IDictionary<string, string> profileLabelsByCode, string subSectionDescription) : base(subSectionDescription)
        {
            ProfileLabelsByCode = profileLabelsByCode;
        }

        public IDictionary<string, string> ProfileLabelsByCode { get; set; }
    }
}
