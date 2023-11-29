using WSOA.Shared.Dtos;
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
    }
}
