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
                tournamentInProgress.WinnableMoneyByPosition = eliminationResult.UpdatedJackpotDistributions.ToDictionary(jac => jac.PlayerPosition, jac => jac.Amount);
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
            tournamentInProgress.WinnableMoneyByPosition = result.UpdatedJackpotDistributions.ToDictionary(jac => jac.PlayerPosition, jac => jac.Amount);
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

        public TournamentInProgressDto Update(PlayerAddonEditionResultDto playerAddonEditionResultDto)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            PlayerPlayingDto playerConcerned = tournamentInProgress.PlayerPlayings.Single(pla => pla.Id == playerAddonEditionResultDto.PlayerUpdated.Id);
            playerConcerned.TotalRebuy = playerAddonEditionResultDto.PlayerUpdated.TotalReBuy;
            playerConcerned.TotalAddOn = playerAddonEditionResultDto.PlayerUpdated.TotalAddOn;

            tournamentInProgress.WinnableMoneyByPosition = playerAddonEditionResultDto.JackpotDistributionsUpdated.ToDictionary(jac => jac.PlayerPosition, jac => jac.Amount);

            return tournamentInProgress;
        }

        public TournamentInProgressDto AddPlayers(AddPlayersResultDto addPlayersResultDto)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            tournamentInProgress.PlayerPlayings = tournamentInProgress.PlayerPlayings.Concat(addPlayersResultDto.AddedPlayersPlaying);

            tournamentInProgress.WinnableMoneyByPosition = addPlayersResultDto.UpdatedJackpotDistributions.ToDictionary(j => j.PlayerPosition, j => j.Amount);

            return tournamentInProgress;
        }

        public TournamentInProgressDto RemovePlayer(int playerId, IEnumerable<JackpotDistribution> jackpotDistributionsUpdated)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            tournamentInProgress.WinnableMoneyByPosition = jackpotDistributionsUpdated.ToDictionary(j => j.PlayerPosition, j => j.Amount);

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

        public TournamentInProgressDto GoToNextStep(TournamentStepEnum newTournamentStep)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            switch (newTournamentStep)
            {
                case TournamentStepEnum.ADDON:
                    tournamentInProgress.IsAddOn = true;
                    foreach (PlayerPlayingDto player in tournamentInProgress.PlayerPlayings)
                    {
                        player.TotalAddOn = 0;
                    }
                    break;
                case TournamentStepEnum.FINAL_TABLE:
                    tournamentInProgress.IsFinalTable = true;
                    break;
            }

            return tournamentInProgress;
        }

        public TournamentInProgressDto GoToPreviousStep(SwitchTournamentStepResultDto switchTournamentStepResult)
        {
            TournamentInProgressDto tournamentInProgress = CheckTournamentAlwaysInProgress();

            switch (switchTournamentStepResult.NewTournamentStep)
            {
                case TournamentStepEnum.ADDON:
                    tournamentInProgress.IsFinalTable = false;
                    break;
                case TournamentStepEnum.NORMAL:
                    tournamentInProgress.IsAddOn = false;
                    foreach (PlayerPlayingDto player in tournamentInProgress.PlayerPlayings)
                    {
                        player.TotalAddOn = null;
                    }
                    tournamentInProgress.WinnableMoneyByPosition = switchTournamentStepResult.UpdatedJackpotDistributions!.ToDictionary(j => j.PlayerPosition, j => j.Amount);
                    break;
            }

            return tournamentInProgress;
        }

        public TournamentInProgressDto UpdateWinnableMoneysByPosition(IEnumerable<JackpotDistribution> jackpotDistributionsUpdated)
        {
            TournamentInProgressDto tournamentInProgressDto = CheckTournamentAlwaysInProgress();

            tournamentInProgressDto.WinnableMoneyByPosition = jackpotDistributionsUpdated.OrderBy(j => j.PlayerPosition).ToDictionary(j => j.PlayerPosition, j => j.Amount);

            return tournamentInProgressDto;
        }
    }
}
