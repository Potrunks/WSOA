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
    public class SaveBonusTournamentEarnedTest : TestClassBase
    {
        private User _performerUsr;
        private User _earnedBonusUsr;
        private Tournament _currentTournament;
        private Player _earnedBonusPlayer;
        private BonusTournament _selectedBonusTournament;
        private BonusTournamentEarnedCreationDto _creationDto;

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private IBonusTournamentEarnedRepository _bonusTournamentEarnedRepository;
        private IUserRepository _userRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            SaveBusinessAction(ProfileResources.ORGANIZER_CODE, BusinessActionResources.EDIT_BONUS_TOURNAMENT_EARNED);
            _performerUsr = SaveUser("Alexis", "ARRIAL", "aarrial", "Trunks92!", ProfileResources.ORGANIZER_CODE);
            _earnedBonusUsr = SaveUser("Antoine", "GUERTS", "aguerts", "aguerts", ProfileResources.PLAYER_CODE);
            _currentTournament = SaveTournament(isInProgress: true);
            _earnedBonusPlayer = SavePlayer(_currentTournament.Id, _earnedBonusUsr.Id, PresenceStateResources.PRESENT_CODE);

            _selectedBonusTournament = SaveBonusTournament("BONUS_TOURNAMENT_TEST", 20);
            _creationDto = CreateBonusTournamentEarnedCreationDto(_earnedBonusPlayer.Id, _selectedBonusTournament);

            _sessionMock = CreateISessionMock(_performerUsr.ProfileCode, _performerUsr.Id);
            _transactionManagerMock = CreateITransactionManagerMock();
            _bonusTournamentEarnedRepository = new BonusTournamentEarnedRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);

            _tournamentBusiness = new TournamentBusiness
            (
                _transactionManagerMock.Object,
                null,
                null,
                null,
                _userRepository,
                null,
                null,
                null,
                null,
                _bonusTournamentEarnedRepository
            );
        }

        [TestMethod]
        public void ShouldAddInDatabaseNewBonusTournamentEarned_WhenPlayerNotAlreadyEarnedIt()
        {
            APICallResult<BonusTournamentEarnedCreationResultDto> result = ExecuteSaveBonusTournamentEarned();

            VerifyTransactionManagerCommit(_transactionManagerMock);
            VerifyAPICallResultSuccess(result, null);
            Assert.AreEqual(_selectedBonusTournament.Code, result.Data.BonusTournamentEarned.BonusTournamentCode);
            Assert.AreEqual(_earnedBonusPlayer.Id, result.Data.BonusTournamentEarned.PlayerId);
            Assert.AreEqual(_selectedBonusTournament.PointAmount, result.Data.BonusTournamentEarned.PointAmount);
            Assert.AreEqual(1, result.Data.BonusTournamentEarned.Occurrence);
        }

        [TestMethod]
        public void ShouldIncreaseOccurenceBonusTournamentEarned_WhenPlayerAlreadyWinBonusTournament()
        {
            BonusTournamentEarned existingBonusTournamentEarned = SaveBonusTournamentEarned(_earnedBonusPlayer.Id, _selectedBonusTournament);

            APICallResult<BonusTournamentEarnedCreationResultDto> result = ExecuteSaveBonusTournamentEarned();

            VerifyTransactionManagerCommit(_transactionManagerMock);
            VerifyAPICallResultSuccess(result, null);
            Assert.AreEqual(existingBonusTournamentEarned.Id, result.Data.BonusTournamentEarned.Id);
            Assert.AreEqual(existingBonusTournamentEarned.BonusTournamentCode, result.Data.BonusTournamentEarned.BonusTournamentCode);
            Assert.AreEqual(existingBonusTournamentEarned.PlayerId, result.Data.BonusTournamentEarned.PlayerId);
            Assert.AreEqual(existingBonusTournamentEarned.PointAmount, result.Data.BonusTournamentEarned.PointAmount);
            Assert.AreEqual(2, result.Data.BonusTournamentEarned.Occurrence);
        }

        [TestMethod]
        public void ShouldNotCreateBonusTournamentEarned_WhenUserNotAbleToPerformAction()
        {
            _performerUsr.ProfileCode = ProfileResources.PLAYER_CODE;
            _dbContext.SaveChanges();
            _sessionMock = CreateISessionMock(_performerUsr.ProfileCode, _performerUsr.Id);
            _tournamentBusiness = new TournamentBusiness
            (
                _transactionManagerMock.Object,
                null,
                null,
                null,
                _userRepository,
                null,
                null,
                null,
                null,
                _bonusTournamentEarnedRepository
            );

            APICallResult<BonusTournamentEarnedCreationResultDto> result = ExecuteSaveBonusTournamentEarned();

            VerifyTransactionManagerRollback(_transactionManagerMock);
            string expectedErrorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg), expectedErrorMsg);
        }

        private APICallResult<BonusTournamentEarnedCreationResultDto> ExecuteSaveBonusTournamentEarned()
        {
            return _tournamentBusiness.SaveBonusTournamentEarned(_creationDto, _sessionMock.Object);
        }
    }
}
