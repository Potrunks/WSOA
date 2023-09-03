﻿using Microsoft.AspNetCore.Http;
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

        private Mock<ISession> _sessionMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<ITournamentRepository> _tournamentRepository;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMailService> _mailServiceMock;

        private ITournamentBusiness _tournamentBusiness;

        [TestInitialize]
        public void Init()
        {
            _form = CreateTournamentCreationFormVM();

            _sessionMock = CreateISessionMock(ProfileResources.ORGANIZER_CODE);

            _transactionManagerMock = CreateITransactionManagerMock();

            _menuRepositoryMock = CreateIMenuRepositoryMock();
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(ProfileResources.ORGANIZER_CODE, It.IsAny<int>()))
                               .Returns(new MainNavSubSection());

            _tournamentRepository = CreateITournamentRepositoryMock();
            _tournamentRepository.Setup(m => m.SaveTournament(It.IsAny<Tournament>()))
                                 .Callback<Tournament>(t => _tournamentCreated = t);

            _userRepositoryMock = CreateIUserRepositoryMock();
            _userRepositoryMock.Setup(m => m.GetAllUsers())
                               .Returns(CreateUsers(3));

            _mailServiceMock = CreateIMailServiceMock();
            _mailServiceMock.Setup(m => m.SendMails(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>()));

            _tournamentBusiness = new TournamentBusiness
            (
                _transactionManagerMock.Object,
                _menuRepositoryMock.Object,
                _tournamentRepository.Object,
                _mailServiceMock.Object,
                _userRepositoryMock.Object
            );
        }

        [TestMethod]
        public void ShouldCreateTournament()
        {
            APICallResult result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerCommit(_transactionManagerMock);
            Assert.AreEqual(_form.Season, _tournamentCreated.Season);
            Assert.AreEqual(_form.StartDate, _tournamentCreated.StartDate);
            Assert.AreEqual(_form.BuyIn, _tournamentCreated.BuyIn);
            Assert.AreEqual(_form.AddressId, _tournamentCreated.AddressId);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateTournament_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null);

            APICallResult result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _tournamentCreated);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_NOT_CONNECTED, result.ErrorMessage);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateTournament_WhenUserCannotPerformAction()
        {
            _sessionMock = CreateISessionMock(ProfileResources.ADMINISTRATOR_CODE);
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.Is<string>(s => s != ProfileResources.ORGANIZER_CODE), It.IsAny<int>()))
                               .Returns(() => null);

            APICallResult result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _tournamentCreated);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, result.ErrorMessage);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_CANNOT_PERFORM_ACTION), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateTournament_WhenStartDateIsPast()
        {
            _form.StartDate = DateTime.UtcNow.AddDays(-7);

            APICallResult result = _tournamentBusiness.CreateTournament(_form, _sessionMock.Object);

            VerifyTransactionManagerRollback(_transactionManagerMock);
            Assert.AreEqual(null, _tournamentCreated);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(DataValidationResources.STARTDATE_PAST_ERROR, result.ErrorMessage);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }
    }
}
