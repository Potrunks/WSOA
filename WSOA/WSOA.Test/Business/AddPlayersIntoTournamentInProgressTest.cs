using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Data.Implementation;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;

namespace WSOA.Test.Business
{
    [TestClass]
    public class AddPlayersIntoTournamentInProgressTest : TestClassBase
    {
        private User _usrPerformer;

        private Tournament _tournamentInProgress;

        private User _usrToAdd;
        private IEnumerable<User> _usersToAdd;

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private IUserRepository _userRepository;
        private IPlayerRepository _playerRepository;
        private ITournamentRepository _tournamentRepository;
        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _usrPerformer = SaveUser("Alexis", "ARRIAL", "aarrial", "Trunks92!", ProfileResources.ORGANIZER_CODE);

            _tournamentInProgress = SaveTournament(true);
            SavePlayers(_tournamentInProgress.Id, PresenceStateResources.PRESENT_CODE, 3);

            _usrToAdd = SaveUser("New", "PLAYER", "nplayer", "Trunks92!", ProfileResources.PLAYER_CODE);
            _usersToAdd = new List<User> { _usrToAdd };

            _sessionMock = CreateISessionMock(_usrPerformer.ProfileCode, _usrPerformer.Id);
            _transactionManagerMock = CreateITransactionManagerMock();
            _userRepository = new UserRepository(_dbContext);
            _playerRepository = new PlayerRepository(_dbContext);
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
                    null,
                    null
                );
        }

        [TestMethod]
        public void ShouldAddPlayersIntoTournamentInProgress()
        {
            APICallResult<IEnumerable<PlayerPlayingDto>> result = ExecuteAddPlayersIntoTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual
                (
                    _usrToAdd,
                    (
                        from usr in _dbContext.Users
                        join pla in _dbContext.Players on usr.Id equals pla.UserId
                        join tou in _dbContext.Tournaments on pla.PlayedTournamentId equals tou.Id
                        where usr.Id == _usrToAdd.Id && tou.Id == _tournamentInProgress.Id
                        select usr
                    ).Single()
                );
            Assert.AreEqual(_usrToAdd.FirstName, result.Data.Single().FirstName);
            Assert.AreEqual(_usrToAdd.LastName, result.Data.Single().LastName);
        }

        [TestMethod]
        public void ShouldNotAddPlayersIntoTournamentInProgress_WhenUserToAddNotExistsInDatabase()
        {
            _usrToAdd.Id = -1;

            APICallResult<IEnumerable<PlayerPlayingDto>> result = ExecuteAddPlayersIntoTournamentInProgress();

            string expectedErrorMsg = UserBusinessMessageResources.USER_NO_EXISTS_IN_DB;
            VerifyAPICallResultError(result, string.Format(RouteResources.MAIN_ERROR, expectedErrorMsg), expectedErrorMsg);
        }

        // erreur si le tournoi n'existe pas

        // Si user pas co
        // Si user a pas les droit

        private APICallResult<IEnumerable<PlayerPlayingDto>> ExecuteAddPlayersIntoTournamentInProgress()
        {
            return _tournamentBusiness.AddPlayersIntoTournamentInProgress(_usersToAdd.Select(usr => usr.Id), _tournamentInProgress.Id, _sessionMock.Object);
        }
    }
}
