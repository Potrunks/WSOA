namespace WSOA.Shared.ViewModel
{
    public class MessageViewModel
    {
        public MessageViewModel()
        {

        }

        public MessageViewModel(PlayerViewModel player)
        {
            Content = $"{char.ToUpper(player.FirstName[0]) + player.FirstName.Substring(1)} {char.ToUpper(player.LastName[0])}.";
        }
        public string Content { get; set; }

        public bool IsError { get; set; }
    }
}
