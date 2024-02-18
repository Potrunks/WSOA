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
    public class LoadTournamentInProgressTest : TestClassBase
    {
        private User _usrProcessor;
        private Tournament _tournamentInProgress;
        private Tournament _tournamentPrevious;
        private List<Player> _playersIntoTournament;
        private List<Player> _playersIntoPreviousTournament;
        private List<BonusTournament> _availableBonus;
        private MainNavSubSection _mainNavSubSection;

        private Mock<ISession> _sessionMock;
        private IMenuRepository _menuRepository;
        private ITournamentRepository _tournamentRepository;
        private IPlayerRepository _playerRepository;
        private IBonusTournamentRepository _bonusTournamentRepository;
        private IUserRepository _userRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _mainNavSubSection = SaveMainNavSubSection(ProfileResources.ORGANIZER_CODE);

            _usrProcessor = SaveUser(nameof(LoadTournamentInProgressTest), "Password");

            _tournamentInProgress = SaveTournament(isInProgress: true);
            _tournamentPrevious = SaveTournament(isOver: true, startDate: _tournamentInProgress.StartDate.AddMonths(-1));

            _playersIntoTournament = SavePlayers(_tournamentInProgress.Id, PresenceStateResources.PRESENT_CODE, 3);
            _playersIntoPreviousTournament = SavePlayers(_tournamentPrevious.Id, PresenceStateResources.PRESENT_CODE, 3, tournamentIsOver: true);

            _availableBonus = SaveBonusTournaments(3);
            SaveBonusTournamentEarneds(_playersIntoTournament.Select(pla => pla.Id), _availableBonus);

            _sessionMock = CreateISessionMock(ProfileResources.ORGANIZER_CODE, _usrProcessor.Id);
            _menuRepository = new MenuRepository(_dbContext);
            _tournamentRepository = new TournamentRepository(_dbContext);
            _playerRepository = new PlayerRepository(_dbContext);
            _bonusTournamentRepository = new BonusTournamentRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);

            _tournamentBusiness = new TournamentBusiness
                (
                    null,
                    _menuRepository,
                    _tournamentRepository,
                    null,
                    _userRepository,
                    null,
                    _playerRepository,
                    _bonusTournamentRepository,
                    null,
                    null,
                    null
                );
        }

        [TestMethod]
        public void LoadTournamentInProgressSuccessfully()
        {
            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);

            Assert.AreEqual(_tournamentInProgress.Id, result.Data.Id);
            Assert.AreEqual(_tournamentInProgress.Season, result.Data.Season);
            Assert.AreEqual(2, result.Data.TournamentNumber);
            Assert.AreEqual(_tournamentInProgress.BuyIn, result.Data.BuyIn);
            Assert.AreEqual(false, result.Data.IsFinalTable);
            Assert.AreEqual(false, result.Data.IsAddOn);
            Assert.AreEqual(1, result.Data.WinnableMoneyByPosition.Single().Key);
            Assert.AreEqual(30, result.Data.WinnableMoneyByPosition.Single().Value);
            foreach (BonusTournament bonus in _availableBonus)
            {
                Assert.AreEqual(bonus, result.Data.WinnableBonus.Single(bon => bon.Code == bonus.Code));
            }
            foreach (PlayerPlayingDto playerResult in result.Data.PlayerPlayings)
            {
                Player player = _playersIntoTournament.Single(pla => pla.Id == playerResult.Id);
                User usr = _dbContext.Users.Single(usr => usr.Id == player.UserId);
                Assert.AreEqual(player.Id, playerResult.Id);
                Assert.AreEqual(player.TotalReBuy, playerResult.TotalRebuy);
                Assert.AreEqual(player.TotalAddOn, playerResult.TotalAddOn);
                Assert.AreEqual(usr.FirstName, playerResult.FirstName);
                Assert.AreEqual(usr.LastName, playerResult.LastName);
                Assert.AreEqual(_availableBonus.Count, playerResult.BonusTournamentEarnedsByBonusTournamentCode.Count);
                Assert.AreEqual(false, playerResult.IsEliminated);
            }
        }

        [TestMethod]
        public void DontLoadTournamentInProgress_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            string expectedErrorMsg = MainBusinessResources.USER_NOT_CONNECTED;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg), expectedErrorMsg);
        }

        [TestMethod]
        public void DontLoadTournamentInProgress_WhenUserCannotPerformAction()
        {
            _sessionMock = CreateISessionMock(ProfileResources.PLAYER_CODE, _usrProcessor.Id);
            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            string expectedErrorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg), expectedErrorMsg);
        }

        [TestMethod]
        public void DontLoadTournamentInProgress_WhenNoTournamentInProgressExists()
        {
            _tournamentInProgress.IsInProgress = false;
            _dbContext.SaveChanges();

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            string expectedErrorMsg = TournamentMessageResources.NO_TOURNAMENT_IN_PROGRESS;
            VerifyAPICallResultError(result, string.Format(RouteResources.MAIN_ERROR, expectedErrorMsg), expectedErrorMsg);
        }

        [TestMethod]
        public void DontLoadTournamentInProgress_WhenNoPlayers()
        {
            foreach (Player player in _playersIntoTournament)
            {
                player.PresenceStateCode = PresenceStateResources.ABSENT_CODE;
            }
            _dbContext.SaveChanges();

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            string expectedErrorMsg = TournamentMessageResources.NO_PLAYERS_PRESENT;
            VerifyAPICallResultError(result, string.Format(RouteResources.MAIN_ERROR, expectedErrorMsg), expectedErrorMsg);
        }

        [TestMethod]
        public void LoadTournamentInProgress_WhenIsFirstTournamentIntoSeason()
        {
            _tournamentPrevious.Season = "Other Season";
            _dbContext.SaveChanges();

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
            Assert.AreEqual(1, result.Data.TournamentNumber);
        }

        [TestMethod]
        public void LoadTournamentInProgress_WhenLastWinnerPlayingIntoCurrentTournamentInProgress()
        {
            Player newPlayer = SavePlayer(_tournamentInProgress.Id, _playersIntoPreviousTournament.Single(pla => pla.CurrentTournamentPosition == 1).UserId, PresenceStateResources.PRESENT_CODE);

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
        }

        [TestMethod]
        public void LoadTournamentInProgress_WhenFirstRankedPlayingIntoCurrentTournamentInProgress()
        {
            Player secondRankedPreviousTournament = _playersIntoPreviousTournament.Single(pla => pla.CurrentTournamentPosition == 2);
            secondRankedPreviousTournament.TotalWinningsPoint = int.MaxValue;
            Player newPlayer = SavePlayer(_tournamentInProgress.Id, secondRankedPreviousTournament.UserId, PresenceStateResources.PRESENT_CODE);

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
        }

        [TestMethod]
        public void LoadTournamentInProgress_WhenIsFinalTable()
        {
            foreach (Player player in _playersIntoTournament)
            {
                player.WasFinalTable = true;
            }
            _dbContext.SaveChanges();

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
            Assert.AreEqual(true, result.Data.IsFinalTable);
        }

        [TestMethod]
        public void LoadTournamentInProgress_WhenIsAddOn()
        {
            foreach (Player player in _playersIntoTournament)
            {
                player.WasAddOn = true;
            }
            _dbContext.SaveChanges();

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
            Assert.AreEqual(true, result.Data.IsAddOn);
        }

        [TestMethod]
        public void LoadTournamentInProgress_WhenSomePlayersIsEliminated()
        {
            Player eliminatedPlayer = _playersIntoTournament.First();
            eliminatedPlayer.CurrentTournamentPosition = _playersIntoTournament.Count;
            _dbContext.SaveChanges();

            APICallResult<TournamentInProgressDto> result = ExecuteLoadTournamentInProgress();

            VerifyAPICallResultSuccess(result, null);
            Assert.AreEqual(eliminatedPlayer.Id, result.Data.PlayerPlayings.Single(pla => pla.IsEliminated).Id);
        }

        private APICallResult<TournamentInProgressDto> ExecuteLoadTournamentInProgress()
        {
            return _tournamentBusiness.LoadTournamentInProgress(_mainNavSubSection.Id, _sessionMock.Object);
        }
    }
}
