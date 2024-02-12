using WSOA.Shared.Entity;
using WSOA.Shared.Utils;

namespace WSOA.Shared.ViewModel
{
    public class PlayerPointViewModel
    {
        public PlayerPointViewModel()
        {

        }

        public PlayerPointViewModel(User user, IEnumerable<Player> players, IEnumerable<BonusTournamentEarned> bonusEarneds)
        {
            Point = players.Sum(pla => pla.TotalWinningsPoint.GetValueOrDefault()) + bonusEarneds.Sum(bon => bon.PointAmount);
            FullName = StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(user.FirstName, user.LastName);
        }

        public int Point { get; set; }

        public string FullName { get; set; }
    }
}
