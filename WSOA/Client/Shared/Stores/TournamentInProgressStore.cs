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
                tournamentInProgress.WinnableMoneyByPosition[1] = tournamentInProgress.WinnableMoneyByPosition[1] + tournamentInProgress.BuyIn;
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

        public TournamentInProgressDto Update(BonusTournamentEarnedEditResultDto result)
        {
            TournamentInProgressDto tournament = GetData()!;

            PlayerPlayingDto player = tournament.PlayerPlayings.Single(player => player.Id == result.EditedBonusTournamentEarned.PlayerId);

            if (player.BonusTournamentEarnedsByBonusTournamentCode.TryGetValue(result.EditedBonusTournamentEarned.BonusTournamentCode, out BonusTournamentEarnedDto? bonusTournamentEarned))
            {
                if (result.EditedBonusTournamentEarned.Occurrence == 0)
                {
                    player.BonusTournamentEarnedsByBonusTournamentCode.Remove(result.EditedBonusTournamentEarned.BonusTournamentCode);
                }
                else
                {
                    bonusTournamentEarned.Occurence = result.EditedBonusTournamentEarned.Occurrence;
                }
            }
            else
            {
                BonusTournament associatedBonusTournament = tournament.WinnableBonus.Single(bonus => bonus.Code == result.EditedBonusTournamentEarned.BonusTournamentCode);
                bonusTournamentEarned = new BonusTournamentEarnedDto(associatedBonusTournament, result.EditedBonusTournamentEarned);
                player.BonusTournamentEarnedsByBonusTournamentCode.Add(associatedBonusTournament.Code, bonusTournamentEarned);
            }

            return tournament;
        }

        public TournamentInProgressDto Update(CancelEliminationResultDto result)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            PlayerPlayingDto eliminatedPlayer = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == result.PlayerEliminated.Id);
            eliminatedPlayer.IsEliminated = false;
            int totalAmountDiff = ((eliminatedPlayer.TotalRebuy ?? 0) - (result.PlayerEliminated.TotalReBuy ?? 0)) * tournamentInProgress.BuyIn;
            int positionCanBeModify = tournamentInProgress.WinnableMoneyByPosition.First(win => win.Value >= totalAmountDiff).Key;
            tournamentInProgress.WinnableMoneyByPosition[positionCanBeModify] = tournamentInProgress.WinnableMoneyByPosition[positionCanBeModify] - totalAmountDiff;
            eliminatedPlayer.TotalRebuy = result.PlayerEliminated.TotalReBuy;


            PlayerPlayingDto eliminatorPlayer = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == result.PlayerEliminator.Id);
            if (result.BonusTournamentsLostByEliminator != null)
            {
                foreach (BonusTournament bonusTournamentLost in result.BonusTournamentsLostByEliminator)
                {
                    eliminatorPlayer.BonusTournamentEarnedsByBonusTournamentCode.Remove(bonusTournamentLost.Code);
                }
            }

            return tournamentInProgress;
        }

        public TournamentInProgressDto Update(Player player)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            int totalJackpotBefore = tournamentInProgress.CalculateTotalJackpot();

            PlayerPlayingDto playerConcerned = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == player.Id);
            playerConcerned.TotalRebuy = player.TotalReBuy;
            playerConcerned.TotalAddOn = player.TotalAddOn;

            int totalJackpotAfter = tournamentInProgress.CalculateTotalJackpot();

            int totalJackpotDiff = totalJackpotBefore - totalJackpotAfter;
            if (totalJackpotDiff > 0)
            {
                int positionCanBeModify = tournamentInProgress.WinnableMoneyByPosition.First(win => win.Value >= totalJackpotDiff).Key;
                tournamentInProgress.WinnableMoneyByPosition[positionCanBeModify] = tournamentInProgress.WinnableMoneyByPosition[positionCanBeModify] - totalJackpotDiff;
            }
            else
            {
                tournamentInProgress.WinnableMoneyByPosition[1] = tournamentInProgress.WinnableMoneyByPosition[1] + Math.Abs(totalJackpotDiff);
            }

            return tournamentInProgress;
        }

        public TournamentInProgressDto RemovePlayer(int playerId)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            int positionCanBeModify = tournamentInProgress.WinnableMoneyByPosition.First(win => win.Value >= tournamentInProgress.BuyIn).Key;
            tournamentInProgress.WinnableMoneyByPosition[positionCanBeModify] = tournamentInProgress.WinnableMoneyByPosition[positionCanBeModify] - tournamentInProgress.BuyIn;

            tournamentInProgress.PlayerPlayings = tournamentInProgress.PlayerPlayings.Where(pla => pla.Id != playerId);

            return tournamentInProgress;
        }

        public TournamentInProgressDto CheckTournamentAlwaysInProgress()
        {
            TournamentInProgressDto? tournamentInProgressDto = GetData();
            if (tournamentInProgressDto == null)
            {
                string errorMsg = TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS;
                string redirectUrl = string.Format(RouteResources.MAIN_ERROR, errorMsg);
                throw new FunctionalException(errorMsg, redirectUrl);
            }
            return tournamentInProgressDto;
        }

        public bool IsAddOn()
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();
            return tournamentInProgress.IsAddOn;
        }

        public bool CanRemovePlayer(int playerId)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();
            PlayerPlayingDto playerConcerned = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == playerId);
            return !tournamentInProgress.IsAddOn
                    && !tournamentInProgress.IsFinalTable
                    && !playerConcerned.BonusTournamentEarnedsByBonusTournamentCode.Any()
                    && !playerConcerned.IsEliminated
                    && playerConcerned.TotalAddOn == null
                    && playerConcerned.TotalRebuy == null;
        }

        public void Clean()
        {
            Data = null;
        }
    }
}
