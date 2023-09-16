using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class PlayerDataViewModel
    {
        public PlayerDataViewModel()
        {

        }

        public PlayerDataViewModel(User user, string presenceStateCode)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            PresenceStateCode = presenceStateCode;
            UserId = user.Id;
        }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PresenceStateCode { get; set; }
    }
}
