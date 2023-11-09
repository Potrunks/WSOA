using WSOA.Shared.Entity;

namespace WSOA.Shared.Dtos
{
    public class TournamentInProgressDto
    {
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
        }

        public int Id { get; set; }

        public string Season { get; set; }

        public int TournamentNumber { get; set; }

        public int BuyIn { get; set; }

        public bool IsFinalTable { get; set; }

        public bool IsAddOn { get; set; }

        public IEnumerable<BonusTournament> WinnableBonus { get; set; }

        public IEnumerable<PlayerPlayingDto> PlayerPlayings { get; set; }
    }
}
