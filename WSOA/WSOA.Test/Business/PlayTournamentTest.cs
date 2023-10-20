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
    public class PlayTournamentTest : TestClassBase
    {
        private Tournament _tournamentTargeted;
        private List<Player> _playersIntoTargetedTournament;
        private IEnumerable<Player> _playersSaved;
        private IEnumerable<Player> _playersDeleted;
        private List<User> _selectedUsrs;
        private User _usrProcessor;
        private TournamentPreparedDto _tournamentPreparedDto;

        private Mock<ISession> _sessionMock;
        private ITournamentRepository _tournamentRepository;
        private IPlayerRepository _playerRepository;
        private Mock<IPlayerRepository> _playerRepositoryMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private IUserRepository _userRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _tournamentTargeted = SaveTournament(1);
            _selectedUsrs = SaveUsers(3);
            _usrProcessor = SaveUser();
            _playersIntoTargetedTournament = SavePlayers(_selectedUsrs.Select(usr => usr.Id), _tournamentTargeted.Id, PresenceStateResources.ABSENT_CODE);
            _tournamentPreparedDto = CreateTournamentPreparedDto(_tournamentTargeted.Id, _selectedUsrs.Select(usr => usr.Id));
            SaveBusinessAction(ProfileResources.ORGANIZER_CODE, "EXEC_TOURNAMENT_FOR_TEST");

            _sessionMock = CreateISessionMock(ProfileResources.ORGANIZER_CODE, _usrProcessor.Id);
            _tournamentRepository = new TournamentRepository(_dbContext);
            _playerRepository = new PlayerRepository(_dbContext);
            _transactionManagerMock = CreateITransactionManagerMock();
            _userRepository = new UserRepository(_dbContext);

            _tournamentBusiness = new TournamentBusiness
                (
                    _transactionManagerMock.Object,
                    null,
                    _tournamentRepository,
                    null,
                    _userRepository,
                    null,
                    _playerRepository
                );
        }

        [TestMethod]
        public void ShouldMarkInProgress_WhenTournamentIsPrepared()
        {
            APICallResultBase result = ExecutePlayTournamentPreparedMethod();

            VerifyAPICallResultSuccess(result, RouteBusinessResources.TOURNAMENT_IN_PROGRESS);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(true, _tournamentTargeted.IsInProgress);
        }

        [TestMethod]
        public void ShouldMarkAsPresentSelectedUsers_WhenAlreadySignUpInTournamentInPreparation()
        {
            APICallResultBase result = ExecutePlayTournamentPreparedMethod();

            VerifyAPICallResultSuccess(result, RouteBusinessResources.TOURNAMENT_IN_PROGRESS);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            foreach (Player player in _playersIntoTargetedTournament)
            {
                Assert.AreEqual(PresenceStateResources.PRESENT_CODE, player.PresenceStateCode);
            }
        }

        [TestMethod]
        public void ShouldCreateNewSelectedPlayer_WhenNotAlreadySignUpInTournamentInPreparation()
        {
            User newSelectedUsrNotSignUp = SaveUser();
            _selectedUsrs.Add(newSelectedUsrNotSignUp);
            _tournamentPreparedDto = CreateTournamentPreparedDto(_tournamentTargeted.Id, _selectedUsrs.Select(usr => usr.Id));

            _playerRepositoryMock = CreateIPlayerRepositoryMock();
            _playerRepositoryMock.Setup(m => m.SavePlayers(It.IsAny<IEnumerable<Player>>()))
                                 .Callback<IEnumerable<Player>>(list => _playersSaved = list);
            _tournamentBusiness = new TournamentBusiness
                (
                    _transactionManagerMock.Object,
                    null,
                    _tournamentRepository,
                    null,
                    _userRepository,
                    null,
                    _playerRepositoryMock.Object
                );

            APICallResultBase result = ExecutePlayTournamentPreparedMethod();

            VerifyAPICallResultSuccess(result, RouteBusinessResources.TOURNAMENT_IN_PROGRESS);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Player newPlayerCreated = _playersSaved.Single(pla => pla.UserId == newSelectedUsrNotSignUp.Id);
            Assert.AreEqual(PresenceStateResources.PRESENT_CODE, newPlayerCreated.PresenceStateCode);
            Assert.AreEqual(_tournamentTargeted.Id, newPlayerCreated.PlayedTournamentId);
        }

        [TestMethod]
        public void ShouldDeletePlayer_WhenNotSelected()
        {
            User usrNotSelected = SaveUser();
            Player playerNotSelected = SavePlayer(_tournamentTargeted.Id, usrNotSelected.Id, PresenceStateResources.PRESENT_CODE);
            _playersIntoTargetedTournament.Add(playerNotSelected);

            _playerRepositoryMock = CreateIPlayerRepositoryMock();
            _playerRepositoryMock.Setup(m => m.DeletePlayers(It.IsAny<IEnumerable<Player>>()))
                                 .Callback<IEnumerable<Player>>(list => _playersDeleted = list);
            _tournamentBusiness = new TournamentBusiness
                (
                    _transactionManagerMock.Object,
                    null,
                    _tournamentRepository,
                    null,
                    _userRepository,
                    null,
                    _playerRepositoryMock.Object
                );

            APICallResultBase result = ExecutePlayTournamentPreparedMethod();

            VerifyAPICallResultSuccess(result, RouteBusinessResources.TOURNAMENT_IN_PROGRESS);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(true, _playersDeleted.All(pla => pla.Id == playerNotSelected.Id));
        }

        [TestMethod]
        public void ShouldNotPlayTournamentPrepared_WhenUserProcessorNotAuthorized()
        {
            _sessionMock = CreateISessionMock(ProfileResources.PLAYER_CODE, _usrProcessor.Id);

            APICallResultBase result = ExecutePlayTournamentPreparedMethod();

            string expectedErrorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg), expectedErrorMsg);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotPlayTournament_WhenTournamentIsAlreadyInProgress()
        {
            _tournamentTargeted.IsInProgress = true;

            APICallResultBase result = ExecutePlayTournamentPreparedMethod();

            string expectedErrorMsg = TournamentBusinessResources.CANNOT_EXECUTE_TOURNAMENT;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg), expectedErrorMsg);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotPlayTournament_WhenTournamentIsAlreadyOver()
        {
            _tournamentTargeted.IsOver = true;

            APICallResultBase result = ExecutePlayTournamentPreparedMethod();

            string expectedErrorMsg = TournamentBusinessResources.CANNOT_EXECUTE_TOURNAMENT;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg), expectedErrorMsg);
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        private APICallResultBase ExecutePlayTournamentPreparedMethod()
        {
            return _tournamentBusiness.PlayTournamentPrepared(_tournamentPreparedDto, _sessionMock.Object);
        }
    }
}
