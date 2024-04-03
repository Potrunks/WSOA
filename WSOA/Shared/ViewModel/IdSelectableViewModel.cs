using WSOA.Shared.Utils;

namespace WSOA.Shared.ViewModel
{
    public class IdSelectableViewModel
    {
        public IdSelectableViewModel() { }

        public IdSelectableViewModel(PlayerViewModel player)
        {
            Id = player.UserId;
            Label = StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(player.FirstName, player.LastName);
        }

        public IdSelectableViewModel(PlayerPlayingViewModel player)
        {
            Id = player.Id;
            Label = StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(player.FirstName, player.LastName);
        }

        public int Id { get; set; }

        public string Label { get; set; }

        public bool IsSelected { get; set; }
    }
}
