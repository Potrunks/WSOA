using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Exceptions;
using WSOA.Shared.Resources;

namespace WSOA.Client.Shared.Stores
{
    public class TournamentInProgressStore
    {
        private TournamentInProgressDto? Data { get; set; }

        public TournamentInProgressDto SetData(TournamentInProgressDto tournamentInProgress)
        {
            Data = tournamentInProgress;
            return Data;
        }

        public TournamentInProgressDto? GetData()
        {
            return Data;
        }

        public TournamentInProgressDto Update(EliminationCreationDto elimination, EliminationCreationResultDto eliminationResult)
        {
            if (eliminationResult.IsTournamentOver)
            {
                Data = null;
                string errorMsg = TournamentMessageResources.TOURNAMENT_FINISHED;
                string redirectUrl = string.Format(RouteResources.MAIN_ERROR, errorMsg);
                throw new FunctionalException(errorMsg, redirectUrl);
            }

            TournamentInProgressDto tournamentInProgress = GetData()!;

            PlayerPlayingDto eliminatedPlayer = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == elimination.EliminatedPlayerId);
            if (elimination.HasReBuy)
            {
                eliminatedPlayer.TotalRebuy = eliminationResult.EliminatedPlayerTotalReBuy;
                tournamentInProgress.TotalJackpot += tournamentInProgress.BuyIn;
                tournamentInProgress.WinnableMoneyByPosition[1] = tournamentInProgress.TotalJackpot;
            }
            else
            {
                eliminatedPlayer.IsEliminated = true;
            }

            PlayerPlayingDto eliminatorPlayer = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == elimination.EliminatorPlayerId);
            if (eliminationResult.EliminatorPlayerWonBonusCodes.Any())
            {
                IEnumerable<BonusTournament> bonusWon = tournamentInProgress.WinnableBonus.Where(b => eliminationResult.EliminatorPlayerWonBonusCodes.Contains(b.Code));
                foreach (BonusTournament bonusTournament in bonusWon)
                {
                    BonusTournamentEarnedDto bonusTournamentEarned = new BonusTournamentEarnedDto(bonusTournament);
                    eliminatorPlayer.BonusTournamentEarnedsByBonusTournamentCode.Add(bonusTournament.Code, bonusTournamentEarned);
                }
            }

            return tournamentInProgress;
        }

        public TournamentInProgressDto Update(BonusTournamentEarnedCreationDto creation, BonusTournamentEarnedCreationResultDto result)
        {
            TournamentInProgressDto tournament = GetData()!;

            PlayerPlayingDto player = tournament.PlayerPlayings.Single(player => player.Id == creation.ConcernedPlayerId);

            if (player.BonusTournamentEarnedsByBonusTournamentCode.TryGetValue(creation.EarnedBonus.Code, out BonusTournamentEarnedDto? bonusTournamentEarned))
            {
                bonusTournamentEarned.Occurence = result.BonusTournamentEarned.Occurrence;
            }
            else
            {
                bonusTournamentEarned = new BonusTournamentEarnedDto(creation.EarnedBonus, result.BonusTournamentEarned);
                player.BonusTournamentEarnedsByBonusTournamentCode.Add(creation.EarnedBonus.Code, bonusTournamentEarned);
            }

            return tournament;
        }
    }
}
