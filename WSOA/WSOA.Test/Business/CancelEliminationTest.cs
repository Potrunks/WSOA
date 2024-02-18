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
    public class CancelEliminationTest : TestClassBase
    {
        private User _usrPerformer;

        private Tournament _tournament;

        private Player _playerEliminated;
        private User _usrEliminated;
        private List<Elimination> _eliminations = new List<Elimination>();

        private Player _playerEliminator;

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private IEliminationRepository _eliminationRepository;
        private ITournamentRepository _tournamentRepository;
        private IUserRepository _userRepository;
        private IBonusTournamentEarnedRepository _bonusTournamentEarnedRepository;
        private IBonusTournamentRepository _bonusTournamentRepository;
        private IPlayerRepository _playerRepository;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _usrPerformer = SaveUser("Aang", "AVATAR", "aavatar", "Trunks92!", ProfileResources.ORGANIZER_CODE);

            _tournament = SaveTournament(true);

            _usrEliminated = SaveUser("Alexis", "ARRIAL", "aarrial", "Trunks92!", ProfileResources.PLAYER_CODE);
            _playerEliminated = SavePlayer(_tournament.Id, _usrEliminated.Id, PresenceStateResources.PRESENT_CODE, totalRebuy: 3);
            _playerEliminator = SavePlayer("Antoine", "GUERTS", ProfileResources.PLAYER_CODE, _tournament.Id, PresenceStateResources.PRESENT_CODE);

            _eliminations.AddRange(SaveEliminations(_playerEliminated.Id, _playerEliminator.Id, 3, false));

            _sessionMock = CreateISessionMock(_usrPerformer.ProfileCode, _usrPerformer.Id);
            _transactionManagerMock = CreateITransactionManagerMock();
            _eliminationRepository = new EliminationRepository(_dbContext);
            _tournamentRepository = new TournamentRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);
            _bonusTournamentEarnedRepository = new BonusTournamentEarnedRepository(_dbContext);
            _bonusTournamentRepository = new BonusTournamentRepository(_dbContext);
            _playerRepository = new PlayerRepository(_dbContext);

            _tournamentBusiness = new TournamentBusiness
            (
                _transactionManagerMock.Object,
                null,
                _tournamentRepository,
                null,
                _userRepository,
                null,
                _playerRepository,
                _bonusTournamentRepository,
                _eliminationRepository,
                _bonusTournamentEarnedRepository,
                null
            );
        }

        [TestMethod]
        public void ShouldCancelLastEliminationAndUpdateRebuy_WhenLastEliminationNotDefinitive()
        {
            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_playerEliminated.Id, result.Data.PlayerEliminated.Id);
            Assert.AreEqual(2, result.Data.PlayerEliminated.TotalReBuy);
            Assert.AreEqual(_playerEliminator.Id, result.Data.PlayerEliminator.Id);
            Assert.AreEqual(null, result.Data.BonusTournamentsLostByEliminator);
            Assert.AreEqual(_eliminations.Last(), result.Data.EliminationCanceled);
            Assert.AreEqual(null, _dbContext.Eliminations.SingleOrDefault(elim => elim.Id == _eliminations.Last().Id));
        }

        [TestMethod]
        public void ShouldCancelLastEliminationAndRePutPlayerIntoTournament_WhenLastEliminationIsDefinitive()
        {
            _eliminations.Add(SaveElimination(_playerEliminated.Id, _playerEliminator.Id, true, _eliminations.Last().CreationDate.AddMinutes(30)));

            _playerEliminated.CurrentTournamentPosition = 3;
            _playerEliminated.WasAddOn = true;
            _playerEliminated.WasFinalTable = true;
            _playerEliminated.TotalWinningsPoint = 100;
            _playerEliminated.TotalWinningsAmount = 1000;

            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_playerEliminated.Id, result.Data.PlayerEliminated.Id);
            Assert.AreEqual(3, result.Data.PlayerEliminated.TotalReBuy);
            Assert.AreEqual(_playerEliminator.Id, result.Data.PlayerEliminator.Id);
            Assert.AreEqual(null, result.Data.BonusTournamentsLostByEliminator);
            Assert.AreEqual(_eliminations.Last(), result.Data.EliminationCanceled);
            Assert.AreEqual(null, _dbContext.Eliminations.SingleOrDefault(elim => elim.Id == _eliminations.Last().Id));
            Assert.AreEqual(null, _playerEliminated.CurrentTournamentPosition);
            Assert.AreEqual(false, _playerEliminated.WasAddOn);
            Assert.AreEqual(false, _playerEliminated.WasFinalTable);
            Assert.AreEqual(null, _playerEliminated.TotalWinningsPoint);
            Assert.AreEqual(null, _playerEliminated.TotalWinningsAmount);
        }

        [TestMethod]
        public void ShouldRemoveFirstRankSeasonBonusOfEliminatorPlayer_WhenEliminatedPlayerHaveDefinitiveEliminationCanceled()
        {
            _eliminations.Add(SaveElimination(_playerEliminated.Id, _playerEliminator.Id, true, _eliminations.Last().CreationDate.AddMinutes(30)));
            Tournament previousTournament = SaveTournament(season: _tournament.Season, startDate: _tournament.StartDate.AddMonths(-1), isOver: true);
            SavePlayer(previousTournament.Id, _usrEliminated.Id, PresenceStateResources.PRESENT_CODE, 2, 1000);
            SavePlayer("Tony", "PORTAIL", ProfileResources.PLAYER_CODE, previousTournament.Id, PresenceStateResources.PRESENT_CODE, 1, 500);
            BonusTournamentEarned eliminatorBonusFirstRanked = SaveBonusTournamentEarned(_playerEliminator.Id, SaveBonusTournament(BonusTournamentResources.FIRST_RANKED_KILLED, 20));

            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(eliminatorBonusFirstRanked.BonusTournamentCode, result.Data.BonusTournamentsLostByEliminator!.Single().Code);
            Assert.AreEqual(null, _dbContext.BonusTournamentEarneds.SingleOrDefault(bon => bon.PlayerId == _playerEliminator.Id));
        }

        [TestMethod]
        public void ShouldRemovePreviousWinnerBonusOfEliminatorPlayer_WhenEliminatedPlayerHaveDefinitiveEliminationCanceled()
        {
            _eliminations.Add(SaveElimination(_playerEliminated.Id, _playerEliminator.Id, true, _eliminations.Last().CreationDate.AddMinutes(30)));
            Tournament previousTournament = SaveTournament(season: _tournament.Season, startDate: _tournament.StartDate.AddMonths(-1), isOver: true);
            SavePlayer(previousTournament.Id, _usrEliminated.Id, PresenceStateResources.PRESENT_CODE, 1, 1000);
            SavePlayer("Tony", "PORTAIL", ProfileResources.PLAYER_CODE, previousTournament.Id, PresenceStateResources.PRESENT_CODE, 2, 500);
            BonusTournamentEarned eliminatorBonusPreviousWinnerKilled = SaveBonusTournamentEarned(_playerEliminator.Id, SaveBonusTournament(BonusTournamentResources.PREVIOUS_WINNER_KILLED, 20));

            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination();

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(eliminatorBonusPreviousWinnerKilled.BonusTournamentCode, result.Data.BonusTournamentsLostByEliminator!.Single().Code);
            Assert.AreEqual(null, _dbContext.BonusTournamentEarneds.SingleOrDefault(bon => bon.PlayerId == _playerEliminator.Id));
        }

        [TestMethod]
        public void ShouldNotRemoveElimination_WhenUserCannotPerformAction()
        {
            _usrPerformer.ProfileCode = ProfileResources.PLAYER_CODE;
            _dbContext.SaveChanges();

            _sessionMock = CreateISessionMock(_usrPerformer.ProfileCode, _usrPerformer.Id);

            _tournamentBusiness = new TournamentBusiness
            (
                _transactionManagerMock.Object,
                null,
                _tournamentRepository,
                null,
                _userRepository,
                null,
                _playerRepository,
                _bonusTournamentRepository,
                _eliminationRepository,
                _bonusTournamentEarnedRepository,
                null
            );

            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination();

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
        public void ShouldSetTotalRebuyNull_WhenTotalRebuyIsZero()
        {
            _playerEliminated.TotalReBuy = 1;
            _dbContext.SaveChanges();

            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination();

            Assert.AreEqual(null, result.Data.PlayerEliminated.TotalReBuy);
        }

        [TestMethod]
        public void ShouldSetPlayerWasAddonTrue_WhenEliminationXasDefinitiveAndTournamentIsAddon()
        {
            _eliminations.Add(SaveElimination(_playerEliminated.Id, _playerEliminator.Id, true, _eliminations.Last().CreationDate.AddMinutes(30)));

            _playerEliminated.CurrentTournamentPosition = 3;
            _playerEliminated.WasAddOn = false;
            _playerEliminated.WasFinalTable = false;
            _playerEliminated.TotalWinningsPoint = 100;
            _playerEliminated.TotalWinningsAmount = 1000;

            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination(true);

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_playerEliminated.Id, result.Data.PlayerEliminated.Id);
            Assert.AreEqual(3, result.Data.PlayerEliminated.TotalReBuy);
            Assert.AreEqual(_playerEliminator.Id, result.Data.PlayerEliminator.Id);
            Assert.AreEqual(null, result.Data.BonusTournamentsLostByEliminator);
            Assert.AreEqual(_eliminations.Last(), result.Data.EliminationCanceled);
            Assert.AreEqual(null, _dbContext.Eliminations.SingleOrDefault(elim => elim.Id == _eliminations.Last().Id));
            Assert.AreEqual(null, _playerEliminated.CurrentTournamentPosition);
            Assert.AreEqual(true, _playerEliminated.WasAddOn);
            Assert.AreEqual(false, _playerEliminated.WasFinalTable);
            Assert.AreEqual(null, _playerEliminated.TotalWinningsPoint);
            Assert.AreEqual(null, _playerEliminated.TotalWinningsAmount);
        }

        [TestMethod]
        public void ShouldSetPlayerWasFinalTableTrue_WhenEliminationXasDefinitiveAndTournamentIsFinalTable()
        {
            _eliminations.Add(SaveElimination(_playerEliminated.Id, _playerEliminator.Id, true, _eliminations.Last().CreationDate.AddMinutes(30)));

            _playerEliminated.CurrentTournamentPosition = 3;
            _playerEliminated.WasAddOn = false;
            _playerEliminated.WasFinalTable = false;
            _playerEliminated.TotalWinningsPoint = 100;
            _playerEliminated.TotalWinningsAmount = 1000;

            APICallResult<CancelEliminationResultDto> result = ExecuteCancelElimination(isFinalTable: true);

            VerifyAPICallResultSuccess(result, null);
            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_playerEliminated.Id, result.Data.PlayerEliminated.Id);
            Assert.AreEqual(3, result.Data.PlayerEliminated.TotalReBuy);
            Assert.AreEqual(_playerEliminator.Id, result.Data.PlayerEliminator.Id);
            Assert.AreEqual(null, result.Data.BonusTournamentsLostByEliminator);
            Assert.AreEqual(_eliminations.Last(), result.Data.EliminationCanceled);
            Assert.AreEqual(null, _dbContext.Eliminations.SingleOrDefault(elim => elim.Id == _eliminations.Last().Id));
            Assert.AreEqual(null, _playerEliminated.CurrentTournamentPosition);
            Assert.AreEqual(false, _playerEliminated.WasAddOn);
            Assert.AreEqual(true, _playerEliminated.WasFinalTable);
            Assert.AreEqual(null, _playerEliminated.TotalWinningsPoint);
            Assert.AreEqual(null, _playerEliminated.TotalWinningsAmount);
        }

        private APICallResult<CancelEliminationResultDto> ExecuteCancelElimination(bool isAddon = false, bool isFinalTable = false)
        {
            EliminationEditionDto eliminationEditionDto = new EliminationEditionDto
            {
                EliminatedPlayerId = _playerEliminated.Id,
                IsAddOn = isAddon,
                IsFinalTable = isFinalTable
            };
            return _tournamentBusiness.CancelLastPlayerElimination(eliminationEditionDto, _sessionMock.Object);
        }
    }
}
