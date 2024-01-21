using WSOA.Shared.Entity;
using WSOA.Shared.Utils;

namespace WSOA.Shared.Dtos
{
    public class TournamentInProgressDto
    {
        public TournamentInProgressDto()
        {

        }

        public TournamentInProgressDto
        (
            Tournament tournament,
            IEnumerable<PlayerDto> players,
            IDictionary<string, BonusTournament> winnableBonusByCode,
            IDictionary<int, IEnumerable<BonusTournamentEarned>> bonusEarnedsByPlayerId,
            int tournamentNb,
            User? lastWinner,
            User? firstRankUser
        )
        {
            Id = tournament.Id;
            Season = tournament.Season;
            TournamentNumber = tournamentNb;
            StartDate = tournament.StartDate;
            BuyIn = tournament.BuyIn;
            IsFinalTable = players.Any(player => player.Player.WasFinalTable.GetValueOrDefault());
            IsAddOn = players.Any(player => player.Player.WasAddOn.GetValueOrDefault());
            WinnableBonus = winnableBonusByCode.Values;
            PlayerPlayings = players.Select(player =>
            {
                IEnumerable<BonusTournamentEarned> currentBonusTournamentEarneds = new List<BonusTournamentEarned>();
                if (bonusEarnedsByPlayerId.TryGetValue(player.Player.Id, out IEnumerable<BonusTournamentEarned> value))
                {
                    currentBonusTournamentEarneds = value;
                }
                return new PlayerPlayingDto(player, winnableBonusByCode, currentBonusTournamentEarneds, lastWinner, firstRankUser);
            });
            WinnableMoneyByPosition = new Dictionary<int, int>
            {
                { 1, CalculateTotalJackpot() }
            };
        }

        public int Id { get; set; }

        public string Season { get; set; }

        public int TournamentNumber { get; set; }

        public DateTime StartDate { get; set; }

        public int BuyIn { get; set; }

        public bool IsFinalTable { get; set; }

        public bool IsAddOn { get; set; }

        public IEnumerable<BonusTournament> WinnableBonus { get; set; }

        public IEnumerable<PlayerPlayingDto> PlayerPlayings { get; set; }

        public IDictionary<int, int> WinnableMoneyByPosition { get; set; }

        public string? GetNameFirstPlayerDefinitivelyEliminated(IEnumerable<int> playerIds)
        {
            string? result = null;
            PlayerPlayingDto? firstPlayerDefinitivelyEliminated = PlayerPlayings.Where(pla => playerIds.Contains(pla.Id)).FirstOrDefault(pla => pla.IsEliminated);
            if (firstPlayerDefinitivelyEliminated != null)
            {
                result = StringFormatUtil.ToFullFirstNameAndFirstLetterLastName(firstPlayerDefinitivelyEliminated.FirstName, firstPlayerDefinitivelyEliminated.LastName);
            }
            return result;
        }

        public int CalculateTotalJackpot()
        {
            return BuyIn * (PlayerPlayings.Count()
                            + PlayerPlayings.Where(pla => pla.TotalRebuy.HasValue && pla.TotalRebuy > 0).Sum(pla => pla.TotalRebuy!.Value)
                            + PlayerPlayings.Where(pla => pla.TotalAddOn.HasValue && pla.TotalAddOn > 0).Sum(pla => pla.TotalAddOn!.Value));
        }
    }
}
