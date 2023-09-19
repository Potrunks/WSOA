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
    public class AccountBusinessTest : TestClassBase
    {
        private SignInFormViewModel _signInFormVM;
        private LinkAccountCreationFormViewModel _linkAccountCreationVM;

        private IAccountBusiness _accountBusiness;
        private Mock<ISession> _sessionMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private Mock<IMailService> _mailServiceMock;

        [TestInitialize]
        public void Init()
        {
            _accountRepositoryMock = CreateIAccountRepositoryMock();
            _userRepositoryMock = CreateIUserRepositoryMock();
            _menuRepositoryMock = CreateIMenuRepositoryMock();
            _transactionManagerMock = CreateITransactionManagerMock();
            _mailServiceMock = CreateIMailServiceMock();
            _accountBusiness = new AccountBusiness(_accountRepositoryMock.Object, _userRepositoryMock.Object, _menuRepositoryMock.Object, _transactionManagerMock.Object, _mailServiceMock.Object);
            _sessionMock = CreateISessionMock(ProfileResources.ADMINISTRATOR_CODE, null);

            _signInFormVM = new SignInFormViewModel
            {
                Login = "Login",
                Password = "Password"
            };

            _linkAccountCreationVM = new LinkAccountCreationFormViewModel
            {
                ProfileCodeSelected = ProfileResources.PLAYER_CODE,
                RecipientMail = "test@test.test",
                SubSectionIdConcerned = 1
            };
        }

        [TestMethod]
        public void ShouldSuccessSignIn_WhenLoginAndPasswordIsOk()
        {
            APICallResult result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(RouteBusinessResources.HOME, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldFailSignIn_WhenFormModelIsNull()
        {
            APICallResult result = _accountBusiness.SignIn(null, _sessionMock.Object);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.TECHNICAL_ERROR, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldFailSignIn_WhenAccountNotFound()
        {
            _accountRepositoryMock.Setup(m => m.GetByLoginAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                                    .Returns(() => null);
            APICallResult result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(AccountBusinessResources.ERROR_SIGN_IN, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldCreateLinkAccountCreation()
        {
            LinkAccountCreation linkCreated = null;
            _accountRepositoryMock.Setup(m => m.SaveLinkAccountCreation(It.IsAny<LinkAccountCreation>()))
                                    .Callback<LinkAccountCreation>((link) => linkCreated = link);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(_linkAccountCreationVM.RecipientMail, linkCreated.RecipientMail);
            Assert.AreEqual(_linkAccountCreationVM.ProfileCodeSelected, linkCreated.ProfileCode);
            Assert.AreEqual(DateTime.UtcNow.AddDays(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY).Day, linkCreated.ExpirationDate.Day);
            _transactionManagerMock.Verify(t => t.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(t => t.CommitTransaction(), Times.Once());
            _mailServiceMock.Verify(m => m.SendMailAccountCreationLink(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ShouldDontCreateLinkAccountCreation_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_NOT_CONNECTED, result.ErrorMessage);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), result.RedirectUrl);
            _transactionManagerMock.Verify(t => t.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(t => t.CommitTransaction(), Times.Never());
            _mailServiceMock.Verify(m => m.SendMailAccountCreationLink(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void ShouldDontCreateLinkAccountCreation_WhenUserNotAuthorized()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.IsAny<string>(), It.IsAny<int>()))
                                .Returns(() => null);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, result.ErrorMessage);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            _transactionManagerMock.Verify(t => t.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(t => t.CommitTransaction(), Times.Never());
            _mailServiceMock.Verify(m => m.SendMailAccountCreationLink(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void ShouldExtendLinkAccountCreation_WhenLinkAlreadyExistsAndOver()
        {
            LinkAccountCreation currentLink = new LinkAccountCreation
            {
                ExpirationDate = DateTime.UtcNow.AddDays(-1),
                Id = 1,
                ProfileCode = ProfileResources.ADMINISTRATOR_CODE,
                RecipientMail = "test@test.test"
            };
            _accountRepositoryMock.Setup(m => m.GetLinkAccountCreationByMail(It.IsAny<string>()))
                                    .Returns(currentLink);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXTENDED, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(DateTime.UtcNow.AddDays(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY).Day, currentLink.ExpirationDate.Day);
            _transactionManagerMock.Verify(t => t.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(t => t.CommitTransaction(), Times.Once());
            _mailServiceMock.Verify(m => m.SendMailAccountCreationLink(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ShouldNotExtendLinkAccountCreation_WhenLinkAlreadyExistsAndNotOver()
        {
            LinkAccountCreation currentLink = new LinkAccountCreation
            {
                ExpirationDate = DateTime.UtcNow.AddDays(1),
                Id = 1,
                ProfileCode = ProfileResources.ADMINISTRATOR_CODE,
                RecipientMail = "test@test.test"
            };
            _accountRepositoryMock.Setup(m => m.GetLinkAccountCreationByMail(It.IsAny<string>()))
                                    .Returns(currentLink);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(AccountBusinessResources.LINK_ACCOUNT_CREATION_RE_SEND, result.WarningMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            _transactionManagerMock.Verify(t => t.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(t => t.CommitTransaction(), Times.Once());
            _mailServiceMock.Verify(m => m.SendMailAccountCreationLink(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ShouldLoadInviteData()
        {
            InviteCallResult result = _accountBusiness.LoadInviteDatas(1, _sessionMock.Object);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(ProfileResources.ADMINISTRATOR_NAME, result.InviteVM.ProfileLabelsByCode.Single().Value);
        }

        [TestMethod]
        public void ShouldNotLoadInviteData_WhenUserNotAuthorized()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.IsAny<string>(), It.IsAny<int>()))
                                .Returns(() => null);

            InviteCallResult result = _accountBusiness.LoadInviteDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(0, result.InviteVM.ProfileLabelsByCode.Count());
        }

        [TestMethod]
        public void ShouldNotLoadInviteData_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null, null);

            InviteCallResult result = _accountBusiness.LoadInviteDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_NOT_CONNECTED, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), result.RedirectUrl);
            Assert.AreEqual(0, result.InviteVM.ProfileLabelsByCode.Count());
        }

        [TestMethod]
        public void ShouldNotLoadInviteData_WhenNoProfilesInDB()
        {
            _userRepositoryMock.Setup(m => m.GetAllProfiles())
                .Returns(new List<Profile>());

            InviteCallResult result = _accountBusiness.LoadInviteDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.TECHNICAL_ERROR, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(0, result.InviteVM.ProfileLabelsByCode.Count());

            _userRepositoryMock.Setup(m => m.GetAllProfiles())
                .Returns(() => null);

            result = _accountBusiness.LoadInviteDatas(1, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.TECHNICAL_ERROR, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
            Assert.AreEqual(0, result.InviteVM.ProfileLabelsByCode.Count());
        }

        [TestMethod]
        public void ShouldNotCreateLinkAccountCreation_WhenMailAlreadyExists()
        {
            _userRepositoryMock.Setup(m => m.ExistsUserByMail(It.IsAny<string>()))
                                .Returns(true);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            _transactionManagerMock.Verify(t => t.CommitTransaction(), Times.Never());
            _transactionManagerMock.Verify(t => t.RollbackTransaction(), Times.Once());
            Assert.AreEqual(UserBusinessResources.MAIL_ALREADY_EXISTS, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }
    }
}
