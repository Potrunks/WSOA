using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Server.Business.Interface
{
    public interface ITournamentBusiness
    {
        /// <summary>
        /// Create new tournament and prevent all users in app.
        /// </summary>
        APICallResultBase CreateTournament(TournamentCreationFormViewModel form, ISession session);

        /// <summary>
        /// Load datas for tournament creation.
        /// </summary>
        APICallResult<TournamentCreationDataViewModel> LoadTournamentCreationDatas(int subSectionId, ISession session);

        /// <summary>
        /// Load Tournaments not over.
        /// </summary>
        APICallResult<TournamentsViewModel> LoadTournamentsNotOver(int subSectionId, ISession session);

        /// <summary>
        /// Sign up the current user to the tournament selected.
        /// </summary>
        APICallResult<PlayerViewModel> SignUpTournament(SignUpTournamentFormViewModel formVM, ISession session);

        /// <summary>
        /// Get present players and available players before execute tournament.
        /// </summary>
        APICallResult<PlayerSelectionViewModel> LoadPlayersForPlayingTournament(int tournamentId, ISession session);

        /// <summary>
        /// Save the tournament prepared and declare it as in progress.
        /// </summary>
        APICallResultBase SaveTournamentPrepared(TournamentPreparedDto tournamentPrepared, ISession session);

        /// <summary>
        /// Load tournament in progress.
        /// </summary>
        APICallResult<TournamentInProgressDto> LoadTournamentInProgress(int subSectionId, ISession session);

        /// <summary>
        /// Eliminate player definitively if player dont take rebuy.
        /// </summary>
        APICallResult<EliminationCreationResultDto> EliminatePlayer(EliminationCreationDto eliminationDto, ISession session);

        /// <summary>
        /// Save bonus tournament earned by the player.
        /// </summary>
        APICallResult<BonusTournamentEarnedEditResultDto> SaveBonusTournamentEarned(BonusTournamentEarnedEditDto bonusEarnedDto, ISession session);

        /// <summary>
        /// Delete bonus tournament earned by the player.
        /// </summary>
        APICallResult<BonusTournamentEarnedEditResultDto> DeleteBonusTournamentEarned(BonusTournamentEarnedEditDto bonusTournamentEarnedEditDto, ISession session);

        /// <summary>
        /// Cancel the last elimination of a player victim id.
        /// </summary>
        APICallResult<CancelEliminationResultDto> CancelLastPlayerElimination(EliminationEditionDto eliminationEditionDto, ISession session);

        /// <summary>
        /// Allow to edit the value of total addon of a player.
        /// </summary>
        APICallResult<PlayerAddonEditionResultDto> EditPlayerTotalAddon(int playerId, int addonNb, ISession session);

        /// <summary>
        /// Remove a player that never come really in the tournament.
        /// </summary>
        APICallResult<IEnumerable<JackpotDistribution>> RemovePlayerNeverComeIntoTournamentInProgress(int playerId, ISession session);

        /// <summary>
        /// Cancel the tournament in progress
        /// </summary>
        APICallResultBase CancelTournamentInProgress(int tournamentInProgressId, ISession session);

        /// <summary>
        /// Add new players into selected tournament in progress.
        /// </summary>
        APICallResult<AddPlayersResultDto> AddPlayersIntoTournamentInProgress(IEnumerable<int> usrIds, int tournamentId, ISession session);

        /// <summary>
        /// Load users can be add into tournament in progress.
        /// </summary>
        APICallResult<PlayerSelectionViewModel> LoadPlayersForPlayingTournamentInProgress(int tournamentId, ISession session);

        /// <summary>
        /// Go to next step for tournament in progress.
        /// </summary>
        APICallResult<TournamentStepEnum> GoToTournamentInProgressNextStep(int tournamentId, ISession session);

        /// <summary>
        /// Go to previous step for tournament in progress.
        /// </summary>
        APICallResult<SwitchTournamentStepResultDto> GoToTournamentInProgressPreviousStep(int tournamentId, ISession session);

        /// <summary>
        /// Delete selected playable tournament.
        /// </summary>
        APICallResultBase DeletePlayableTournament(int tournamentIdToDelete, ISession session);

        /// <summary>
        /// Load season result by selected season.
        /// </summary>
        APICallResult<SeasonResultViewModel> LoadSeasonResult(int season, ISession session);

        /// <summary>
        /// Edit winnable moneys by position during a tournament in progress.
        /// </summary>
        APICallResult<IEnumerable<JackpotDistribution>> EditWinnableMoneysByPosition(IDictionary<int, int> winnableMoneysByPosition, int tournamentId, ISession session);
    }
}
