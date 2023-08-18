namespace WSOA.Shared.ViewModel
{
    public class InviteViewModel
    {
        public InviteViewModel()
        {
            ProfileLabelsByCode = new Dictionary<string, string>();
            SubSectionDescription = null;
        }

        public IDictionary<string, string> ProfileLabelsByCode { get; set; }

        public string SubSectionDescription { get; set; }
    }
}
