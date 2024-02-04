using WSOA.Shared.Entity;

namespace WSOA.Shared.ViewModel
{
    public class PlayerProfitabilityViewModel
    {
        public PlayerProfitabilityViewModel()
        {

        }

        public PlayerProfitabilityViewModel(User user, IEnumerable<Player> players, IEnumerable<Tournament> tournaments)
        {
            FullName = $"{user.FirstName} {user.LastName}";

            MoneySpent = 0;
            MoneyEarned = 0;
            foreach (Tournament tournament in tournaments)
            {
                MoneySpent = MoneySpent + tournament.BuyIn;
                Player player = players.Single(pla => pla.PlayedTournamentId == tournament.Id && pla.UserId == user.Id);
                MoneySpent = MoneySpent + (player.TotalAddOn.GetValueOrDefault() * tournament.BuyIn) + (player.TotalReBuy.GetValueOrDefault() * tournament.BuyIn);
                MoneyEarned = MoneyEarned + player.TotalWinningsAmount.GetValueOrDefault();
            }

            MoneyProfitability = MoneyEarned - MoneySpent;
        }

        public int MoneySpent { get; set; }

        public int MoneyEarned { get; set; }

        public int MoneyProfitability { get; set; }

        public string FullName { get; set; }
    }
}
