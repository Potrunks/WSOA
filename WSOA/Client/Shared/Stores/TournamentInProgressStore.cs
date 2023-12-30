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

            PlayerPlayingDto playerConcerned = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == player.Id);
            playerConcerned.TotalRebuy = player.TotalReBuy;
            playerConcerned.TotalAddOn = player.TotalAddOn;

            return tournamentInProgress;
        }

        public TournamentInProgressDto RemovePlayer(int playerId)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

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
    }
}
