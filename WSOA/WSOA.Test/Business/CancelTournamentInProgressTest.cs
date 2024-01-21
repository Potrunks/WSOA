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
    public class CancelTournamentInProgressTest : TestClassBase
    {
        private User _usrPerformer;

        private Tournament _tournamentInProgress;

        private Player _player1;
        private Player _player2;
        private Player _player3;
        private List<Player> _players;

        private Mock<ITransactionManager> _transactionManagerMock;
        private Mock<ISession> _sessionMock;
        private ITournamentRepository _tournamentRepository;
        private IPlayerRepository _playerRepository;
        private IEliminationRepository _eliminationRepository;
        private IBonusTournamentEarnedRepository _bonusTournamentEarnedRepository;
        private IUserRepository _userRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _usrPerformer = SaveUser("Alexis", "ARRIAL", "aarrial", "Trunks92!", ProfileResources.ORGANIZER_CODE);

            _tournamentInProgress = SaveTournament(true);

            _player1 = SavePlayer
                (
                    "Player1",
                    "PLAYER1",
                    ProfileResources.PLAYER_CODE,
                    _tournamentInProgress.Id,
                    PresenceStateResources.PRESENT_CODE,
                    totalRebuy: 1,
                    wasAddon: true,
                    totalAddon: 2,
                    wasFinalTable: true
                );

            _player2 = SavePlayer
                (
                    "Player2",
                    "PLAYER2",
                    ProfileResources.PLAYER_CODE,
                    _tournamentInProgress.Id,
                    PresenceStateResources.PRESENT_CODE,
                    totalRebuy: 2,
                    wasAddon: true,
                    totalAddon: 1,
                    wasFinalTable: true
                );

            _player3 = SavePlayer
                (
                    "Player3",
                    "PLAYER3",
                    ProfileResources.PLAYER_CODE,
                    _tournamentInProgress.Id,
                    PresenceStateResources.PRESENT_CODE,
                    totalRebuy: 1,
                    wasAddon: true,
                    totalAddon: 2,
                    wasFinalTable: true,
                    totalPoints: 100,
                    positionInTournament: 3,
                    totalWinningsAmount: 50
                );

            _players = new List<Player> { _player1, _player2, _player3 };

            SaveElimination(_player3.Id, _player2.Id, false);
            SaveElimination(_player2.Id, _player1.Id, false);
            SaveElimination(_player1.Id, _player3.Id, false);

            BonusTournament bonusTournament = _dbContext.BonusTournaments.First(b => b.Code == BonusTournamentResources.STRAIGHT_FLUSH);
            SaveBonusTournamentEarned(_player1.Id, bonusTournament);
            SaveBonusTournamentEarned(_player2.Id, bonusTournament);
            SaveBonusTournamentEarned(_player3.Id, bonusTournament);

            _sessionMock = CreateISessionMock(_usrPerformer.ProfileCode, _usrPerformer.Id);

            _transactionManagerMock = CreateITransactionManagerMock();
            _tournamentRepository = new TournamentRepository(_dbContext);
            _playerRepository = new PlayerRepository(_dbContext);
            _eliminationRepository = new EliminationRepository(_dbContext);
            _bonusTournamentEarnedRepository = new BonusTournamentEarnedRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);

            _tournamentBusiness = new TournamentBusiness
                (
                    _transactionManagerMock.Object,
                    null,
                    _tournamentRepository,
                    null,
                    _userRepository,
                    null,
                    _playerRepository,
                    null,
                    _eliminationRepository,
                    _bonusTournamentEarnedRepository
                );
        }

        [TestMethod]
        public void ShouldCancelTournamentInProgress()
        {
            APICallResultBase result = ExecuteCancelTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(false, _tournamentInProgress.IsInProgress);
            foreach (Player player in _players)
            {
                Assert.AreEqual(null, player.TotalWinningsPoint);
                Assert.AreEqual(null, player.CurrentTournamentPosition);
                Assert.AreEqual(null, player.TotalReBuy);
                Assert.AreEqual(null, player.TotalAddOn);
                Assert.AreEqual(null, player.WasFinalTable);
                Assert.AreEqual(null, player.WasAddOn);
                Assert.AreEqual(null, player.TotalWinningsAmount);
            }
            IEnumerable<int> playerIds = _players.Select(p => p.Id);
            Assert.AreEqual(false, _dbContext.Eliminations.Any(e => playerIds.Contains(e.PlayerEliminatorId) || playerIds.Contains(e.PlayerVictimId)));
            Assert.AreEqual(false, _dbContext.BonusTournamentEarneds.Any(b => playerIds.Contains(b.PlayerId)));
        }

        [TestMethod]
        public void ShouldNotCancelTournamentInProgress_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            APICallResultBase result = ExecuteCancelTournamentInProgress();

            string expectedErrorMsg = MainBusinessResources.USER_NOT_CONNECTED;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg), expectedErrorMsg);
        }

        [TestMethod]
        public void ShouldNotCancelTournamentInProgress_WhenUserCannotPerformAction()
        {
            _usrPerformer.ProfileCode = ProfileResources.PLAYER_CODE;
            _dbContext.SaveChanges();
            _sessionMock = CreateISessionMock(_usrPerformer.ProfileCode, _usrPerformer.Id);
            APICallResultBase result = ExecuteCancelTournamentInProgress();

            string expectedErrorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg), expectedErrorMsg);
        }

        [TestMethod]
        public void ShouldNotCancelTournamentInProgress_WhenTournamentNotInProgress()
        {
            _tournamentInProgress.IsInProgress = false;
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteCancelTournamentInProgress();

            string expectedErrorMsg = TournamentBusinessResources.TOURNAMENT_NOT_IN_PROGRESS;
            VerifyAPICallResultError(result, null, expectedErrorMsg);
        }

        private APICallResultBase ExecuteCancelTournamentInProgress()
        {
            return _tournamentBusiness.CancelTournamentInProgress(_tournamentInProgress.Id, _sessionMock.Object);
        }
    }
}
