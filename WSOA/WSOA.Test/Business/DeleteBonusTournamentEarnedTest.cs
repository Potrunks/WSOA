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
    public class DeleteBonusTournamentEarnedTest : TestClassBase
    {
        private Tournament _tournament;
        private Player _playerEarnedBonus;
        private User _userPerformingAction;
        private BonusTournament _bonusTournament;
        private BonusTournamentEarned _bonusTournamentEarned;
        private BonusTournamentEarnedEditDto _bonusTournamentEarnedEditDto;

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private IBonusTournamentEarnedRepository _bonusTournamentEarnedRepository;
        private IUserRepository _userRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _userPerformingAction = SaveUser("Antoine", "GUERTS", "AGuerts", "Trunks92!", ProfileResources.ORGANIZER_CODE);
            _tournament = SaveTournament(true);
            _playerEarnedBonus = SavePlayer("Alexis", "ARRIAL", ProfileResources.PLAYER_CODE, _tournament.Id, PresenceStateResources.PRESENT_CODE);
            _bonusTournament = SaveBonusTournament("+2AGI", 100);
            _bonusTournamentEarned = SaveBonusTournamentEarned(_playerEarnedBonus.Id, _bonusTournament, 2);
            _bonusTournamentEarnedEditDto = CreateBonusTournamentEarnedEditDto(_playerEarnedBonus.Id, _bonusTournament);

            _sessionMock = CreateISessionMock(_userPerformingAction.ProfileCode, _userPerformingAction.Id);
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
        public void ShouldReduceOccurence_WhenBonusTournamentSelectedToBeDeleted()
        {
            APICallResult<BonusTournamentEarnedEditResultDto> result = ExecuteDeleteBonusTournamentEarned();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_bonusTournamentEarned.Id, result.Data.EditedBonusTournamentEarned.Id);
            Assert.AreEqual(_bonusTournamentEarned.BonusTournamentCode, result.Data.EditedBonusTournamentEarned.BonusTournamentCode);
            Assert.AreEqual(_bonusTournamentEarned.PlayerId, result.Data.EditedBonusTournamentEarned.PlayerId);
            Assert.AreEqual(_bonusTournamentEarned.PointAmount, result.Data.EditedBonusTournamentEarned.PointAmount);
            Assert.AreEqual(1, result.Data.EditedBonusTournamentEarned.Occurrence);
        }

        [TestMethod]
        public void ShouldDeleteBonusTournamentEarned_WhenOccurenceBecomeZero()
        {
            _bonusTournamentEarned.Occurrence = 1;
            _dbContext.SaveChanges();

            APICallResult<BonusTournamentEarnedEditResultDto> result = ExecuteDeleteBonusTournamentEarned();
            BonusTournamentEarned? bonusTournamentEarnedUpdatedInDb = _dbContext.BonusTournamentEarneds.SingleOrDefault(bonus => bonus.Id == _bonusTournamentEarned.Id);

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(null, bonusTournamentEarnedUpdatedInDb);
            Assert.AreEqual(_bonusTournamentEarned.Id, result.Data.EditedBonusTournamentEarned.Id);
            Assert.AreEqual(_bonusTournamentEarned.BonusTournamentCode, result.Data.EditedBonusTournamentEarned.BonusTournamentCode);
            Assert.AreEqual(_bonusTournamentEarned.PlayerId, result.Data.EditedBonusTournamentEarned.PlayerId);
            Assert.AreEqual(_bonusTournamentEarned.PointAmount, result.Data.EditedBonusTournamentEarned.PointAmount);
            Assert.AreEqual(0, result.Data.EditedBonusTournamentEarned.Occurrence);
        }

        [TestMethod]
        public void ShouldNotReduceOrDeleteBonusTournamentEarned_WhenUserCannotPerformAction()
        {
            _userPerformingAction.ProfileCode = ProfileResources.PLAYER_CODE;
            _dbContext.SaveChanges();
            _sessionMock = CreateISessionMock(_userPerformingAction.ProfileCode, _userPerformingAction.Id);
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

            APICallResult<BonusTournamentEarnedEditResultDto> result = ExecuteDeleteBonusTournamentEarned();

            VerifyTransactionManagerRollback(_transactionManagerMock);
            string expectedErrorMsg = MainBusinessResources.USER_CANNOT_PERFORM_ACTION;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, expectedErrorMsg), expectedErrorMsg);
        }

        [TestMethod]
        public void ShouldError_WhenBonusTournamentEarnedToDeleteNotFoundInDb()
        {
            _dbContext.BonusTournamentEarneds.Remove(_bonusTournamentEarned);
            _dbContext.SaveChanges();

            APICallResult<BonusTournamentEarnedEditResultDto> result = ExecuteDeleteBonusTournamentEarned();

            VerifyTransactionManagerRollback(_transactionManagerMock);
            string expectedErrorMsg = MainBusinessResources.TECHNICAL_ERROR;
            VerifyAPICallResultError(result, string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg), expectedErrorMsg);
        }

        private APICallResult<BonusTournamentEarnedEditResultDto> ExecuteDeleteBonusTournamentEarned()
        {
            return _tournamentBusiness.DeleteBonusTournamentEarned(_bonusTournamentEarnedEditDto, _sessionMock.Object);
        }
    }
}
