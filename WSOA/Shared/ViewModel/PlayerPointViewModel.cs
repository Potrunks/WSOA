using WSOA.Shared.Entity;
using WSOA.Shared.Utils;

namespace WSOA.Shared.ViewModel
{
    public class PlayerPointViewModel
    {
        public PlayerPointViewModel()
        {

        }

        public PlayerPointViewModel(User user, IEnumerable<Player> players)
        {
            Point = players.Where(pla => pla.UserId == user.Id).Sum(pla => pla.TotalWinningsPoint.GetValueOrDefault());
            FullName = StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(user.FirstName, user.LastName);
        }

        public int Point { get; set; }

        public string FullName { get; set; }
    }
}
