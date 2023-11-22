using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Test.Business
{
    [TestClass]
    public class SignUpTournamentTest : TestClassBase
    {
        private ITournamentBusiness _tournamentBusiness;

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;

        private User _currentUser;
        private Player? _currentPlayer;
        private Player _createdPlayer;
        private SignUpTournamentFormViewModel _formVM;
        private Tournament _currentTournament;

        [TestInitialize]
        public void Init()
        {
            _currentTournament = CreateTournament(1, 0);
            _formVM = CreateSignUpTournamentFormViewModel(_currentTournament.Id, PresenceStateResources.PRESENT_CODE);

            _currentUser = CreateUser(1, 1);
            _sessionMock = CreateISessionMock(_currentUser.ProfileCode, _currentUser.Id);

            _transactionManagerMock = CreateITransactionManagerMock();

            _tournamentRepositoryMock = CreateITournamentRepositoryMock();
            _tournamentRepositoryMock.Setup(m => m.GetTournamentById(It.IsAny<int>()))
                                     .Returns(_currentTournament);

            _currentPlayer = null;
            _playerRepositoryMock = CreateIPlayerRepositoryMock();
            _playerRepositoryMock.Setup(m => m.GetPlayerByTournamentIdAndUserId(It.IsAny<int>(), It.IsAny<int>()))
                                 .Returns(_currentPlayer);
            _playerRepositoryMock.Setup(m => m.SavePlayer(It.IsAny<Player>()))
                                 .Callback<Player>(p => _createdPlayer = p);

            _userRepositoryMock = CreateIUserRepositoryMock();
            _userRepositoryMock.Setup(m => m.GetUserById(It.IsAny<int>()))
                               .Returns(_currentUser);

            _tournamentBusiness = new TournamentBusiness
                (
                    _transactionManagerMock.Object,
                    null,
                    _tournamentRepositoryMock.Object,
                    null,
                    _userRepositoryMock.Object,
                    null,
                    _playerRepositoryMock.Object,
                    null,
                    null,
                    null
                );
        }

        [TestMethod]
        public void ShouldCreatePlayerAndSignUpTournament_WhenPlayerSignUpTournamentFirstTime()
        {
            APICallResult<PlayerViewModel> result = _tournamentBusiness.SignUpTournament(_formVM, _sessionMock.Object);

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(1, _createdPlayer.PlayedTournamentId);
            Assert.AreEqual(_currentUser.Id, _createdPlayer.UserId);
            Assert.AreEqual(PresenceStateResources.PRESENT_CODE, _createdPlayer.PresenceStateCode);
            Assert.AreEqual(_currentUser.FirstName, result.Data.FirstName);
            Assert.AreEqual(_currentUser.LastName, result.Data.LastName);
            Assert.AreEqual(PresenceStateResources.PRESENT_CODE, result.Data.PresenceStateCode);
            Assert.AreEqual(_currentUser.Id, result.Data.UserId);
        }

        [TestMethod]
        public void ShouldNotSignUpPlayer_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            APICallResult<PlayerViewModel> result = _tournamentBusiness.SignUpTournament(_formVM, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), MainBusinessResources.USER_NOT_CONNECTED);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotSignUpPlayer_WhenTournamentNotExists()
        {
            _tournamentRepositoryMock.Setup(m => m.GetTournamentById(It.IsAny<int>()))
                                     .Returns(() => throw new Exception("Le tournoi n'existe pas"));

            APICallResult<PlayerViewModel> result = _tournamentBusiness.SignUpTournament(_formVM, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.MAIN_ERROR, MainBusinessResources.TECHNICAL_ERROR), MainBusinessResources.TECHNICAL_ERROR);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotSignUpPlayer_WhenSaveSignUpPlayerFailed()
        {
            _playerRepositoryMock.Setup(m => m.SavePlayer(It.IsAny<Player>()))
                                 .Throws(() => new Exception("Erreur pendant la sauvegarde du joueur en base de données"));

            APICallResult<PlayerViewModel> result = _tournamentBusiness.SignUpTournament(_formVM, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.MAIN_ERROR, MainBusinessResources.TECHNICAL_ERROR), MainBusinessResources.TECHNICAL_ERROR);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotSignUpPlayer_WhenGetCurrentUserIdFailed()
        {
            _userRepositoryMock.Setup(m => m.GetUserById(It.IsAny<int>()))
                                 .Throws(() => new Exception("L'utilisateur n'existe pas en base de données"));

            APICallResult<PlayerViewModel> result = _tournamentBusiness.SignUpTournament(_formVM, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.MAIN_ERROR, MainBusinessResources.TECHNICAL_ERROR), MainBusinessResources.TECHNICAL_ERROR);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldSignUpPlayer_WhenAlreadySignUp()
        {
            _currentPlayer = CreatePlayer(1, presenceStateCode: PresenceStateResources.ABSENT_CODE);

            APICallResult<PlayerViewModel> result = _tournamentBusiness.SignUpTournament(_formVM, _sessionMock.Object);

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(1, _createdPlayer.PlayedTournamentId);
            Assert.AreEqual(_currentUser.Id, _createdPlayer.UserId);
            Assert.AreEqual(PresenceStateResources.PRESENT_CODE, _createdPlayer.PresenceStateCode);
            Assert.AreEqual(_currentUser.FirstName, result.Data.FirstName);
            Assert.AreEqual(_currentUser.LastName, result.Data.LastName);
            Assert.AreEqual(PresenceStateResources.PRESENT_CODE, result.Data.PresenceStateCode);
            Assert.AreEqual(_currentUser.Id, result.Data.UserId);
        }

        [TestMethod]
        public void ShouldNotSignUpPlayer_WhenTournamentAlreadyOver()
        {
            _currentTournament.IsOver = true;

            APICallResult<PlayerViewModel> result = _tournamentBusiness.SignUpTournament(_formVM, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.MAIN_ERROR, MainBusinessResources.TECHNICAL_ERROR), MainBusinessResources.TECHNICAL_ERROR);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }
    }
}
