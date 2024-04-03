using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class AddressDataViewModel
    {
        public AddressDataViewModel()
        {

        }

        public AddressDataViewModel(Address address)
        {
            Id = address.Id;
            Content = address.Content;
        }

        public int Id { get; set; }

        public string Content { get; set; }
    }
}
