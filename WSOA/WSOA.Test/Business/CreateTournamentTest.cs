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
    public class CreateTournamentTest : TestClassBase
    {
        private TournamentCreationFormViewModel _form;
        private Tournament _tournamentCreated;
        private IEnumerable<string> _expectedSeasons;
        private List<Address> _expectedAddresses;
        private MainNavSubSection _mainNavSubSectionConcerned;

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<ITournamentRepository> _tournamentRepository;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMailService> _mailServiceMock;
        private Mock<IAddressRepository> _addressRepositoryMock;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _form = CreateTournamentCreationFormVM();

            int currentYear = DateTime.UtcNow.Year;
            _expectedSeasons = new List<string>
            {
                (currentYear--).ToString(),
                currentYear.ToString(),
                SeasonResources.OUT_OF_SEASON
            };

            _expectedAddresses = CreateAddresses(3);

            _mainNavSubSectionConcerned = CreateMainNavSubSection();

            _sessionMock = CreateISessionMock(ProfileResources.ORGANIZER_CODE, null);

            _transactionManagerMock = CreateITransactionManagerMock();

            _menuRepositoryMock = CreateIMenuRepositoryMock();
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(ProfileResources.ORGANIZER_CODE, It.IsAny<int>()))
                               .Returns(_mainNavSubSectionConcerned);

            _tournamentRepository = CreateITournamentRepositoryMock();
            _tournamentRepository.Setup(m => m.SaveTournament(It.IsAny<Tournament>()))
                                 .Callback<Tournament>(t => _tournamentCreated = t);

            _userRepositoryMock = CreateIUserRepositoryMock();
            _userRepositoryMock.Setup(m => m.GetAllUsers(null))
                               .Returns(CreateUsers(3));

            _mailServiceMock = CreateIMailServiceMock();
            _mailServiceMock.Setup(m => m.SendMails(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>()));

            _addressRepositoryMock = CreateIAddressRepositoryMock();
            _addressRepositoryMock.Setup(m => m.GetAllAddresses())
                                  .Returns(_expectedAddresses);

            _tournamentBusiness = new TournamentBusiness
            (
                _transactionManagerMock.Object,
                _menuRepositoryMock.Object,
                _tournamentRepository.Object,
                _mailServiceMock.Object,
                _userRepositoryMock.Object,
                _addressRepositoryMock.Object,
                null,
                null,
                null,
                null,
                null
            );
        }

        [TestMethod]
        public void ShouldCreateTournament()
        {
            APICallResultBase result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_form.Season, _tournamentCreated.Season);
            Assert.AreEqual(_form.StartDate, _tournamentCreated.StartDate);
            Assert.AreEqual(_form.BuyIn, _tournamentCreated.BuyIn);
            Assert.AreEqual(_form.AddressId, _tournamentCreated.AddressId);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateTournament_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            APICallResultBase result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _tournamentCreated);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_NOT_CONNECTED, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateTournament_WhenUserCannotPerformAction()
        {
            _sessionMock = CreateISessionMock(ProfileResources.ADMINISTRATOR_CODE, null);
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.Is<string>(s => s != ProfileResources.ORGANIZER_CODE), It.IsAny<int>()))
                               .Returns(() => null);

            APICallResultBase result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _tournamentCreated);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_CANNOT_PERFORM_ACTION), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateTournament_WhenStartDateIsPast()
        {
            _form.StartDate = DateTime.UtcNow.AddDays(-7);

            APICallResultBase result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _tournamentCreated);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(DataValidationResources.STARTDATE_PAST_ERROR, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldLoadTournamentCreationDatas()
        {
            APICallResult<TournamentCreationDataViewModel> result = _tournamentBusiness.LoadTournamentCreationDatas(1, _sessionMock.Object);
            TournamentCreationDataViewModel viewModelResult = result.Data;

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(true, viewModelResult.SelectableSeasons.All(sea => _expectedSeasons.Any(exp => exp == sea)));
            Assert.AreEqual(true, viewModelResult.SelectableAddresses.All(sea => _expectedAddresses.Any(exp => exp.Id == sea.Id)));
            Assert.AreEqual(_mainNavSubSectionConcerned.Description, viewModelResult.Description);
        }

        [TestMethod]
        public void ShouldDontLoadTournamentCreationDatas_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            APICallResult<TournamentCreationDataViewModel> result = _tournamentBusiness.LoadTournamentCreationDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_NOT_CONNECTED, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), result.RedirectUrl);
            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void ShouldNotLoadTournamentCreationDatas_WhenUserCannotPerformAction()
        {
            _sessionMock = CreateISessionMock(ProfileResources.ADMINISTRATOR_CODE, null);
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.Is<string>(s => s != ProfileResources.ORGANIZER_CODE), It.IsAny<int>()))
                               .Returns(() => null);

            APICallResult<TournamentCreationDataViewModel> result = _tournamentBusiness.LoadTournamentCreationDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_CANNOT_PERFORM_ACTION), result.RedirectUrl);
            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void ShouldDontLoadTournamentCreationDatas_WhenAddressesIsEmptyInDb()
        {
            _addressRepositoryMock.Setup(m => m.GetAllAddresses())
                                  .Returns(new List<Address>());

            APICallResult<TournamentCreationDataViewModel> result = _tournamentBusiness.LoadTournamentCreationDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            string expectedErrorMsg = string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, "addresses", nameof(TournamentBusiness.LoadTournamentCreationDatas));
            Assert.AreEqual(expectedErrorMsg, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg), result.RedirectUrl);
            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void ShouldDontLoadTournamentCreationDatas_WhenAddressesIsNullInDb()
        {
            _addressRepositoryMock.Setup(m => m.GetAllAddresses())
                                  .Returns(() => null);

            APICallResult<TournamentCreationDataViewModel> result = _tournamentBusiness.LoadTournamentCreationDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            string expectedErrorMsg = string.Format(MainBusinessResources.NULL_OR_EMPTY_OBJ_NOT_ALLOWED, "addresses", nameof(TournamentBusiness.LoadTournamentCreationDatas));
            Assert.AreEqual(expectedErrorMsg, result.ErrorMessage);
            Assert.AreEqual(string.Empty, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.MAIN_ERROR, expectedErrorMsg), result.RedirectUrl);
            Assert.AreEqual(null, result.Data);
        }
    }
}
