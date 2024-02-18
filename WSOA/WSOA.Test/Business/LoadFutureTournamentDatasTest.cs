using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Test.Business
{
    [TestClass]
    public class LoadFutureTournamentDatasTest : TestClassBase
    {
        private ITournamentBusiness _tournamentBusiness;

        private Mock<ISession> _sessionMock;
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<ITournamentRepository> _tournamentRepositoryMock;

        private string _currentProfileCode;
        private int _currentUserId;
        private PlayerDto _currentPlayerDto;
        private List<TournamentDto> _tournamentDtos;

        [TestInitialize]
        public void Init()
        {
            _currentProfileCode = ProfileResources.PLAYER_CODE;
            _currentUserId = 1;
            _sessionMock = CreateISessionMock(_currentProfileCode, _currentUserId);

            _menuRepositoryMock = CreateIMenuRepositoryMock();
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.IsAny<string>(), It.IsAny<int>()))
                               .Returns(new MainNavSubSection());

            _tournamentDtos = CreateTournamentDtos(1, 2).ToList();
            _currentPlayerDto = _tournamentDtos.Single().Players.Single(p => p.User.Id == _currentUserId);
            _currentPlayerDto.Player.PresenceStateCode = PresenceStateResources.MAYBE_CODE;
            _tournamentRepositoryMock = CreateITournamentRepositoryMock();
            _tournamentRepositoryMock.Setup(m => m.GetTournamentDtosByIsOverAndIsInProgress(false, false))
                                     .Returns(_tournamentDtos);

            _tournamentBusiness = new TournamentBusiness
                (
                    null,
                    _menuRepositoryMock.Object,
                    _tournamentRepositoryMock.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                );
        }

        [TestMethod]
        public void ShouldLoadFutureTournamentDatas()
        {
            APICallResult<TournamentsViewModel> result = _tournamentBusiness.LoadTournamentsNotOver(1, _sessionMock.Object);

            VerifyAPICallResultSuccess(result, null);
            Assert.AreEqual(_tournamentDtos[0].Tournament.Id, result.Data.TournamentsVM[0].TournamentId);
            Assert.AreEqual(_tournamentDtos[0].Tournament.Season, result.Data.TournamentsVM[0].Season);
            Assert.AreEqual(_tournamentDtos[0].Tournament.StartDate, result.Data.TournamentsVM[0].StartDate);
            Assert.AreEqual(_tournamentDtos[0].Tournament.BuyIn, result.Data.TournamentsVM[0].BuyIn);
            Assert.AreEqual(_tournamentDtos[0].Address.Content, result.Data.TournamentsVM[0].Address);
            Assert.AreEqual(_tournamentDtos[0].Players.Count(), result.Data.TournamentsVM[0].PlayerDatasVM.Count());
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[0].User.Id, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[0].UserId);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[0].User.FirstName, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[0].FirstName);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[0].User.LastName, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[0].LastName);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[0].Player.PresenceStateCode, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[0].PresenceStateCode);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[1].User.Id, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[1].UserId);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[1].User.FirstName, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[1].FirstName);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[1].User.LastName, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[1].LastName);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[1].Player.PresenceStateCode, result.Data.TournamentsVM[0].PlayerDatasVM.ToList()[1].PresenceStateCode);
            Assert.AreEqual(_tournamentDtos[0].Players.ToList()[0].Player.PresenceStateCode, result.Data.TournamentsVM[0].CurrentUserPresenceStateCode);
        }

        [TestMethod]
        public void ShouldNotLoadFutureTournamentDatas_WhenUserProfileNotInSession()
        {
            _sessionMock = CreateISessionMock(null, _currentUserId);

            APICallResult<TournamentsViewModel> result = _tournamentBusiness.LoadTournamentsNotOver(1, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), MainBusinessResources.USER_NOT_CONNECTED);
        }

        [TestMethod]
        public void ShouldNotLoadFutureTournamentDatas_WhenUserCannotPerformAction()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.IsAny<string>(), It.IsAny<int>()))
                               .Returns(() => null);

            APICallResult<TournamentsViewModel> result = _tournamentBusiness.LoadTournamentsNotOver(1, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_CANNOT_PERFORM_ACTION), MainBusinessResources.USER_CANNOT_PERFORM_ACTION);
        }

        [TestMethod]
        public void ShouldNotLoadFutureTournamentDatas_WhenUserIdNotInSession()
        {
            _sessionMock = CreateISessionMock(_currentProfileCode, null);

            APICallResult<TournamentsViewModel> result = _tournamentBusiness.LoadTournamentsNotOver(1, _sessionMock.Object);

            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), MainBusinessResources.USER_NOT_CONNECTED);
        }
    }
}
