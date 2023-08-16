namespace WSOA.Shared.ViewModel
{
    public class InviteViewModel
    {
        public InviteViewModel()
        {
            ProfileLabelsByCode = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ProfileLabelsByCode { get; set; }
    }
}
