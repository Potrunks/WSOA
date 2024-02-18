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
    public class EditPlayerTotalAddonTest : TestClassBase
    {
        private User _userPerformer;
        private Player _playerConcerned;
        private int _totalAddon = 3;

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private IPlayerRepository _playerRepository;
        private IUserRepository _userRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _userPerformer = SaveUser("Test", "TEST", "ttest", "Trunks92!", ProfileResources.ORGANIZER_CODE);
            SaveBusinessAction(_userPerformer.ProfileCode, BusinessActionResources.EDIT_TOTAL_ADDON);
            Tournament tournament = SaveTournament(true);
            _playerConcerned = SavePlayer("Alexis", "ARRIAL", ProfileResources.PLAYER_CODE, tournament.Id, PresenceStateResources.PRESENT_CODE, wasAddon: true);

            _sessionMock = CreateISessionMock(_userPerformer.ProfileCode, _userPerformer.Id);
            _transactionManagerMock = CreateITransactionManagerMock();
            _playerRepository = new PlayerRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);

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
                    null,
                    null
                );
        }

        [TestMethod]
        public void ShouldEditPlayerTotalAddonValue_WhenNewValueGiven()
        {
            APICallResult<PlayerAddonEditionResultDto> result = ExecuteEditPlayerTotalAddon();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_playerConcerned.Id, result.Data.PlayerUpdated.Id);
            Assert.AreEqual(_totalAddon, result.Data.PlayerUpdated.TotalAddOn);
        }

        [TestMethod]
        public void ShouldNotEditPlayerTotalAddon_WhenUserCannotPerformAction()
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
                    null,
                    null
                );

            APICallResult<PlayerAddonEditionResultDto> result = ExecuteEditPlayerTotalAddon();

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
        public void ShouldNotEditPlayerTotalAddon_WhenValueLessThanZero()
        {
            _totalAddon = -1;

            APICallResult<PlayerAddonEditionResultDto> result = ExecuteEditPlayerTotalAddon();

            string expectedErrorMsg = TournamentBusinessResources.TOTAL_ADDON_GIVEN_LESS_THAN_ZERO;
            VerifyAPICallResultError
                (
                    result,
                    null,
                    expectedErrorMsg
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotEditPlayerTotalAddon_WhenPlayerNotFoundIntoDb()
        {
            int deletedPlayerId = _playerConcerned.Id;
            _dbContext.Players.Remove(_playerConcerned);
            _dbContext.SaveChanges();

            APICallResult<PlayerAddonEditionResultDto> result = _tournamentBusiness.EditPlayerTotalAddon(deletedPlayerId, _totalAddon, _sessionMock.Object);

            string expectedErrorMsg = MainBusinessResources.TECHNICAL_ERROR;
            VerifyAPICallResultError
                (
                    result,
                    string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg),
                    expectedErrorMsg
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        [TestMethod]
        public void ShouldNotEditPlayerTotalAddon_WhenPlayerNotIntoAddon()
        {
            _playerConcerned.WasAddOn = false;
            _dbContext.SaveChanges();

            APICallResult<PlayerAddonEditionResultDto> result = ExecuteEditPlayerTotalAddon();

            string expectedErrorMsg = TournamentBusinessResources.PLAYER_NOT_IN_ADDON;
            VerifyAPICallResultError
                (
                    result,
                    null,
                    expectedErrorMsg
                );
            VerifyTransactionManagerRollback(_transactionManagerMock);
        }

        private APICallResult<PlayerAddonEditionResultDto> ExecuteEditPlayerTotalAddon()
        {
            return _tournamentBusiness.EditPlayerTotalAddon(_playerConcerned.Id, _totalAddon, _sessionMock.Object);
        }
    }
}
