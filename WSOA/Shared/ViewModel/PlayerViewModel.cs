using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class PlayerViewModel
    {
        public PlayerViewModel()
        {

        }

        public PlayerViewModel(User user, Player? player = null)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            PresenceStateCode = player?.PresenceStateCode;
            PlayerId = player?.Id;
            UserId = user.Id;
        }

        public int UserId { get; set; }

        public int? PlayerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? PresenceStateCode { get; set; }

        public bool HasPaid { get; set; }

        public bool IsPreSelected { get; set; }
    }
}
