namespace WSOA.Shared.ViewModel
{
    public class ItemSelectableViewModel
    {
        public ItemSelectableViewModel() { }

        public ItemSelectableViewModel(PlayerViewModel player)
        {
            Id = player.UserId;
            Label = $"{char.ToUpper(player.FirstName[0]) + player.FirstName.Substring(1)} {char.ToUpper(player.LastName[0])}.";
        }

        public int Id { get; set; }

        public string Label { get; set; }

        public bool IsSelected { get; set; }
    }
}
