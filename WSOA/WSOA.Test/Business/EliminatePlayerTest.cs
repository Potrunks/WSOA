using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Implementation;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;

namespace WSOA.Test.Business
{
    [TestClass]
    public class EliminatePlayerTest : TestClassBase
    {
        private Tournament _tournamentInProgress;

        private User _currentUser;

        private User _eliminatedUser;
        private Player _eliminatedPlayer;

        private User _eliminatorUser;
        private Player _eliminatorPlayer;

        private User _otherUser;
        private Player _otherPlayer;

        private EliminationDto _eliminationDto;

        private Elimination _eliminationSaved;

        private Mock<ISession> _sessionMock;
        private IUserRepository _userRepository;
        private IPlayerRepository _playerRepository;
        private Mock<IEliminationRepository> _eliminationRepositoryMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private ITournamentRepository _tournamentRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _tournamentInProgress = SaveTournament(isInProgress: true);

            _currentUser = SaveUser
                (
                    "Winston",
                    "WINSTON",
                    "WWinston",
                    "Continental",
                    ProfileResources.ORGANIZER_CODE
                );

            _eliminatedUser = SaveUser
                (
                    "John",
                    "WICK",
                    "JWick",
                    "Assassin",
                    ProfileResources.PLAYER_CODE
                );
            _eliminatedPlayer = SavePlayer
                (
                    _tournamentInProgress.Id,
                    _eliminatedUser.Id,
                    PresenceStateResources.PRESENT_CODE
                );

            _eliminatorUser = SaveUser
                (
                    "Jason",
                    "BOURNE",
                    "JBourne",
                    "Espion",
                    ProfileResources.PLAYER_CODE
                );
            _eliminatorPlayer = SavePlayer
                (
                    _tournamentInProgress.Id,
                    _eliminatorUser.Id,
                    PresenceStateResources.PRESENT_CODE
                );

            _otherUser = SaveUser
                (
                    "Other",
                    "OTHER",
                    "OOther",
                    "Other",
                    ProfileResources.PLAYER_CODE
                );
            _otherPlayer = SavePlayer
                (
                    _tournamentInProgress.Id,
                    _otherUser.Id,
                    PresenceStateResources.PRESENT_CODE
                );

            _eliminationDto = new EliminationDto
            {
                EliminatedPlayerId = _eliminatedPlayer.Id,
                EliminatorPlayerId = _eliminatorPlayer.Id,
                HasReBuy = true,
                WinnableMoneyByPosition = new Dictionary<int, int>
                {
                    { 1, 20 },
                    { 2, 10 }
                }
            };

            _sessionMock = CreateISessionMock(_currentUser.ProfileCode, _currentUser.Id);

            _eliminationRepositoryMock = CreateIEliminationRepositoryMock();
            _eliminationRepositoryMock.Setup(m => m.SaveElimination(It.IsAny<Elimination>()))
                                      .Callback<Elimination>((elimination) =>
                                      {
                                          _eliminationSaved = elimination;
                                      });

            _userRepository = new UserRepository(_dbContext);
            _playerRepository = new PlayerRepository(_dbContext);
            _transactionManagerMock = CreateITransactionManagerMock();
            _tournamentRepository = new TournamentRepository(_dbContext);

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
                    _eliminationRepositoryMock.Object
                );
        }

        [TestMethod]
        public void DontDefinitivelyEliminatePlayer_WhenReBuy()
        {
            APICallResultBase result = ExecuteEliminatePlayer();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(1, _eliminatedPlayer.TotalReBuy);
            Assert.AreEqual(false, _eliminationSaved.IsDefinitive);
            Assert.AreEqual(_eliminatedPlayer.Id, _eliminationSaved.PlayerVictimId);
            Assert.AreEqual(_eliminatorPlayer.Id, _eliminationSaved.PlayerEliminatorId);
            Assert.AreEqual(null, _eliminatedPlayer.TotalWinningsPoint);
            Assert.AreEqual(null, _eliminatedPlayer.CurrentTournamentPosition);
            Assert.AreEqual(null, _eliminatedPlayer.TotalWinningsAmount);
            Assert.AreEqual(true, _tournamentInProgress.IsInProgress);
            Assert.AreEqual(false, _tournamentInProgress.IsOver);
        }

        [TestMethod]
        public void DefinitivelyEliminatePlayer_WhenNoReBuy()
        {
            _eliminationDto.HasReBuy = false;

            APICallResultBase result = ExecuteEliminatePlayer();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(0, _eliminatedPlayer.TotalReBuy);
            Assert.AreEqual(true, _eliminationSaved.IsDefinitive);
            Assert.AreEqual(_eliminatedPlayer.Id, _eliminationSaved.PlayerVictimId);
            Assert.AreEqual(_eliminatorPlayer.Id, _eliminationSaved.PlayerEliminatorId);
            Assert.AreEqual(TournamentPointsResources.TournamentPointAmountByPosition[3], _eliminatedPlayer.TotalWinningsPoint);
            Assert.AreEqual(3, _eliminatedPlayer.CurrentTournamentPosition);
            Assert.AreEqual(0, _eliminatedPlayer.TotalWinningsAmount);
            Assert.AreEqual(true, _tournamentInProgress.IsInProgress);
            Assert.AreEqual(false, _tournamentInProgress.IsOver);
        }

        [TestMethod]
        public void DefinitivelyEliminatePlayerAndWinMoney_WhenNoReBuyAndRanked()
        {
            _otherPlayer.CurrentTournamentPosition = 3;
            _otherPlayer.TotalWinningsPoint = TournamentPointsResources.TournamentPointAmountByPosition[3];
            _otherPlayer.TotalWinningsAmount = 0;
            _dbContext.SaveChanges();
            _eliminationDto.HasReBuy = false;

            APICallResultBase result = ExecuteEliminatePlayer();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(0, _eliminatedPlayer.TotalReBuy);
            Assert.AreEqual(true, _eliminationSaved.IsDefinitive);
            Assert.AreEqual(_eliminatedPlayer.Id, _eliminationSaved.PlayerVictimId);
            Assert.AreEqual(_eliminatorPlayer.Id, _eliminationSaved.PlayerEliminatorId);
            Assert.AreEqual(TournamentPointsResources.TournamentPointAmountByPosition[2], _eliminatedPlayer.TotalWinningsPoint);
            Assert.AreEqual(2, _eliminatedPlayer.CurrentTournamentPosition);
            Assert.AreEqual(10, _eliminatedPlayer.TotalWinningsAmount);
            Assert.AreEqual(false, _tournamentInProgress.IsInProgress);
            Assert.AreEqual(true, _tournamentInProgress.IsOver);
        }

        [TestMethod]
        public void TotalRebuyIsNull_WhenPlayerDefinitivelyEliminatedAndNoRebuyAndIsAddon()
        {
            _eliminatedPlayer.WasAddOn = true;
            _dbContext.SaveChanges();
            _eliminationDto.HasReBuy = false;

            APICallResultBase result = ExecuteEliminatePlayer();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(null, _eliminatedPlayer.TotalReBuy);
            Assert.AreEqual(true, _eliminationSaved.IsDefinitive);
            Assert.AreEqual(_eliminatedPlayer.Id, _eliminationSaved.PlayerVictimId);
            Assert.AreEqual(_eliminatorPlayer.Id, _eliminationSaved.PlayerEliminatorId);
            Assert.AreEqual(TournamentPointsResources.TournamentPointAmountByPosition[3], _eliminatedPlayer.TotalWinningsPoint);
            Assert.AreEqual(3, _eliminatedPlayer.CurrentTournamentPosition);
            Assert.AreEqual(0, _eliminatedPlayer.TotalWinningsAmount);
            Assert.AreEqual(true, _tournamentInProgress.IsInProgress);
            Assert.AreEqual(false, _tournamentInProgress.IsOver);
        }

        [TestMethod]
        public void StopTournament_WhenOnlyTwoPlayerAndOneIsDefinitivelyEliminated()
        {
            _otherPlayer.CurrentTournamentPosition = 3;
            _otherPlayer.TotalWinningsPoint = TournamentPointsResources.TournamentPointAmountByPosition[3];
            _otherPlayer.TotalWinningsAmount = 0;
            _dbContext.SaveChanges();
            _eliminationDto.HasReBuy = false;

            APICallResultBase result = ExecuteEliminatePlayer();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(0, _eliminatedPlayer.TotalReBuy);
            Assert.AreEqual(true, _eliminationSaved.IsDefinitive);
            Assert.AreEqual(_eliminatedPlayer.Id, _eliminationSaved.PlayerVictimId);
            Assert.AreEqual(_eliminatorPlayer.Id, _eliminationSaved.PlayerEliminatorId);
            Assert.AreEqual(TournamentPointsResources.TournamentPointAmountByPosition[2], _eliminatedPlayer.TotalWinningsPoint);
            Assert.AreEqual(2, _eliminatedPlayer.CurrentTournamentPosition);
            Assert.AreEqual(10, _eliminatedPlayer.TotalWinningsAmount);
            Assert.AreEqual(TournamentPointsResources.TournamentPointAmountByPosition[1], _eliminatorPlayer.TotalWinningsPoint);
            Assert.AreEqual(1, _eliminatorPlayer.CurrentTournamentPosition);
            Assert.AreEqual(20, _eliminatorPlayer.TotalWinningsAmount);
            Assert.AreEqual(false, _tournamentInProgress.IsInProgress);
            Assert.AreEqual(true, _tournamentInProgress.IsOver);
        }

        [TestMethod]
        public void DontEliminatePlayer_WhenCannotPerformAction()
        {
            _currentUser.ProfileCode = ProfileResources.PLAYER_CODE;
            _dbContext.SaveChanges();

            _sessionMock = CreateISessionMock(_currentUser.ProfileCode, _currentUser.Id);

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
                    _eliminationRepositoryMock.Object
                );

            APICallResultBase result = ExecuteEliminatePlayer();

            string expectedErrorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
            VerifyAPICallResultError
                (
                    result,
                    string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg),
                    expectedErrorMsg
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _eliminationSaved);
        }

        [TestMethod]
        public void DontEliminatePlayer_WhenEliminatedPlayerNotFoundInDb()
        {
            _dbContext.Players.Remove(_eliminatedPlayer);
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteEliminatePlayer();

            string expectedErrorMsg = MainBusinessResources.TECHNICAL_ERROR;
            VerifyAPICallResultError
                (
                    result,
                    string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg),
                    expectedErrorMsg
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _eliminationSaved);
        }

        [TestMethod]
        public void DontEliminatePlayer_WhenEliminatorPlayerNotFoundInDb()
        {
            _dbContext.Players.Remove(_eliminatorPlayer);
            _dbContext.SaveChanges();

            APICallResultBase result = ExecuteEliminatePlayer();

            string expectedErrorMsg = MainBusinessResources.TECHNICAL_ERROR;
            VerifyAPICallResultError
                (
                    result,
                    string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg),
                    expectedErrorMsg
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _eliminationSaved);
        }

        [TestMethod]
        public void DontEliminatePlayer_WhenAtLeastOnePlayerIsAlreadyDefinitivelyEliminated()
        {
            SaveElimination(_eliminatedPlayer.Id, _eliminatorPlayer.Id, true);
            _eliminationRepositoryMock.Setup(m => m.GetEliminationsByPlayerVictimIds(It.IsAny<IEnumerable<int>>()))
                                      .Returns(_dbContext.Eliminations.Where(elim => new List<int> { _eliminatedPlayer.Id, _eliminatorPlayer.Id }.Contains(elim.PlayerVictimId)));

            APICallResultBase result = ExecuteEliminatePlayer();

            string expectedErrorMsg = TournamentMessageResources.PLAYERS_ALREADY_DEFINITIVELY_ELIMINATED;
            VerifyAPICallResultError(result, null, expectedErrorMsg);
            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _eliminationSaved);
        }

        private APICallResultBase ExecuteEliminatePlayer()
        {
            return _tournamentBusiness.EliminatePlayer(_eliminationDto, _sessionMock.Object);
        }
    }
}
