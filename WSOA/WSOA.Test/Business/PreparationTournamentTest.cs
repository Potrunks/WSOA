using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Test.Business
{
    [TestClass]
    public class PreparationTournamentTest : TestClassBase
    {
        private ITournamentBusiness _tournamentBusiness;

        private User _currentUser;
        private List<PlayerDto> _presentPlayers;
        private List<User> _availableUsers;

        private Mock<ISession> _sessionMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;

        [TestInitialize]
        public void Init()
        {
            _currentUser = CreateUser(1, 1);
            _sessionMock = CreateISessionMock(_currentUser.ProfileCode, _currentUser.Id);

            _presentPlayers = CreatePlayerDtos(1).ToList();
            _playerRepositoryMock = CreateIPlayerRepositoryMock();
            _playerRepositoryMock.Setup(m => m.GetPlayersByTournamentIdAndPresenceStateCode(It.IsAny<int>(), It.IsAny<string>()))
                                 .Returns(_presentPlayers);

            _availableUsers = new List<User> { CreateUser(2, 2) };
            _userRepositoryMock = CreateIUserRepositoryMock();
            _userRepositoryMock.Setup(m => m.GetAllUsers(null))
                               .Returns(_availableUsers);

            _tournamentRepositoryMock = CreateITournamentRepositoryMock();

            _tournamentBusiness = new TournamentBusiness
            (
                null,
                null,
                _tournamentRepositoryMock.Object,
                null,
                _userRepositoryMock.Object,
                null,
                _playerRepositoryMock.Object,
                null
            );
        }

        [TestMethod]
        public void ShouldGetPlayers_ForTournamentPreparation()
        {
            APICallResult<PlayerSelectionViewModel> result = _tournamentBusiness.LoadPlayersForPlayingTournament(1, _sessionMock.Object);

            VerifyAPICallResultSuccess(result, null);
        }

        [TestMethod]
        public void ShouldDontGetPlayersForTournamentPreparation_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            APICallResult<PlayerSelectionViewModel> result = _tournamentBusiness.LoadPlayersForPlayingTournament(1, _sessionMock.Object);

            string errorMsgExpected = MainBusinessResources.USER_NOT_CONNECTED;
            string redirectUrlExpected = string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, errorMsgExpected);

            VerifyAPICallResultError(result, redirectUrlExpected, errorMsgExpected);
        }

        [TestMethod]
        public void ShouldDontGetPlayersForTournamentPreparation_WhenUserCannotExecuteTournament()
        {
            _tournamentRepositoryMock.Setup(m => m.ExistsTournamentByTournamentIdIsOverAndIsInProgress(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                                     .Returns(false);

            APICallResult<PlayerSelectionViewModel> result = _tournamentBusiness.LoadPlayersForPlayingTournament(1, _sessionMock.Object);

            string errorMsgExpected = TournamentBusinessResources.CANNOT_EXECUTE_TOURNAMENT;
            string redirectUrlExpected = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsgExpected);

            VerifyAPICallResultError(result, redirectUrlExpected, errorMsgExpected);
        }

        [TestMethod]
        public void ShouldDontGetPlayersForTournamentPreparation_WhenAlreadyTournamentInProgress()
        {
            _tournamentRepositoryMock.Setup(m => m.ExistsTournamentByIsInProgress(It.IsAny<bool>()))
                                     .Returns(true);

            APICallResult<PlayerSelectionViewModel> result = _tournamentBusiness.LoadPlayersForPlayingTournament(1, _sessionMock.Object);

            string errorMsgExpected = TournamentBusinessResources.EXISTS_TOURNAMENT_IN_PROGRESS;
            string redirectUrlExpected = string.Format(RouteBusinessResources.MAIN_ERROR, errorMsgExpected);

            VerifyAPICallResultError(result, redirectUrlExpected, errorMsgExpected);
        }
    }
}
