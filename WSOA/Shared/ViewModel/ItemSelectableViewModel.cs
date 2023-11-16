using WSOA.Shared.Utils;

namespace WSOA.Shared.ViewModel
{
    public class ItemSelectableViewModel
    {
        public ItemSelectableViewModel() { }

        public ItemSelectableViewModel(PlayerViewModel player)
        {
            Id = player.UserId;
            Label = StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(player.FirstName, player.LastName);
        }

        public ItemSelectableViewModel(PlayerPlayingViewModel player)
        {
            Id = player.Id;
            Label = StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(player.FirstName, player.LastName);
        }

        public int Id { get; set; }

        public string Label { get; set; }

        public bool IsSelected { get; set; }
    }
}
