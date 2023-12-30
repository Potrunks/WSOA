using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Implementation;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;

namespace WSOA.Test.Business
{
    [TestClass]
    public class RemovePlayerNeverComeIntoTournamentInProgressTest : TestClassBase
    {
        private User _userPerformer;
        private Tournament _tournamentConcerned;
        private Player _playerNeverCome;

        private Mock<ITransactionManager> _transactionManagerMock;
        private Mock<ISession> _sessionMock;
        private IPlayerRepository _playerRepository;
        private IUserRepository _userRepository;
        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _userPerformer = SaveUser("Antoine", "GUERTS", "aguerts", "Trunks92!", ProfileResources.ORGANIZER_CODE);
            SaveBusinessAction(_userPerformer.ProfileCode, BusinessActionResources.EDIT_PLAYER_PRESENCE);
            _tournamentConcerned = SaveTournament(true);
            _playerNeverCome = SavePlayer("Alexis", "ARRIAL", ProfileResources.PLAYER_CODE, _tournamentConcerned.Id, PresenceStateResources.PRESENT_CODE);

            _transactionManagerMock = CreateITransactionManagerMock();
            _sessionMock = CreateISessionMock(_userPerformer.ProfileCode, _userPerformer.Id);
            _playerRepository = new PlayerRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);

            _tournamentBusiness = new TournamentBusiness(
                    _transactionManagerMock.Object,
                    null,
                    null,
                    null,
                    _userRepository,
                    null,
                    _playerRepository,
                    null,
                    null,
                    null
                );
        }

        [TestMethod]
        public void ShouldRemovePlayer_WhenNeverComeIntoTournament()
        {
            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(PresenceStateResources.ABSENT_CODE, _playerNeverCome.PresenceStateCode);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenUserCannotPerformAction()
        {
            _userPerformer.ProfileCode = ProfileResources.PLAYER_CODE;
            _dbContext.SaveChanges();

            _sessionMock = CreateISessionMock(_userPerformer.ProfileCode, _userPerformer.Id);

            _tournamentBusiness = new TournamentBusiness
            (
                _transactionManagerMock.Object,
                null,
                null,
                null,
                _userRepository,
                null,
                _playerRepository,
                null,
                null,
                null
            );

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            string expectedErrorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
            VerifyAPICallResultError
                (
                    result,
                    string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg),
                    expectedErrorMsg
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenHaveAlreadyRebuy()
        {
            _playerNeverCome.TotalReBuy = 3;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_REBUY_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldRemovePlayer_WhenPlayerNotMarkedAsPresent()
        {
            _playerNeverCome.PresenceStateCode = PresenceStateResources.ABSENT_CODE;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(PresenceStateResources.ABSENT_CODE, _playerNeverCome.PresenceStateCode);
            Assert.AreEqual(TournamentMessageResources.PLAYER_ALREADY_NOT_PRESENT, result.WarningMessage);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(null, result.ErrorMessage);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenHaveAlreadyAddon()
        {
            _playerNeverCome.TotalAddOn = 3;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_ADDON_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenWasFinalTable()
        {
            _playerNeverCome.WasFinalTable = true;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_FINAL_TABLE_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenPlayerHaveAlreadyEarnedPoints()
        {
            _playerNeverCome.TotalWinningsPoint = 3;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_PTS_EARNED_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenPlayerHaveAlreadyEarnedPosition()
        {
            _playerNeverCome.CurrentTournamentPosition = 3;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_POSITION_EARNED_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenTournamentNotInProgress()
        {
            _tournamentConcerned.IsInProgress = false;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_IN_PROGRESS_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenPlayerConcernedHaveAlreadyEliminatedPlayerIntoTournamentConcerned()
        {
            Player otherPlayer = SavePlayer("Tony", "PORTAIL", ProfileResources.PLAYER_CODE, _tournamentConcerned.Id, PresenceStateResources.PRESENT_CODE);
            SaveElimination(otherPlayer.Id, _playerNeverCome.Id, false);

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_ELIMINATOR_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotRemovePlayer_WhenPlayerConcernedHaveAlreadyEarnedBonus()
        {
            SaveBonusTournamentEarned(_playerNeverCome.Id, _dbContext.BonusTournaments.First());

            APICallResultBase result = ExecuteRemovePlayerNeverComeIntoTournament();

            VerifyAPICallResultError
                (
                    result,
                    null,
                    TournamentMessageResources.REMOVE_PLAYER_FROM_TOURNAMENT_BONUS_ERROR
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        private APICallResultBase ExecuteRemovePlayerNeverComeIntoTournament()
        {
            return _tournamentBusiness.RemovePlayerNeverComeIntoTournamentInProgress(_playerNeverCome.Id, _sessionMock.Object);
        }
    }
}
