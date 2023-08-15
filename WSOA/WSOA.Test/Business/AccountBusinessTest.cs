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
        private LinkAccountCreationViewModel _linkAccountCreationVM;

        private IAccountBusiness _accountBusiness;
        private Mock<ISession> _sessionMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMenuRepository> _menuRepositoryMock;

        [TestInitialize]
        public void Init()
        {
            _accountRepositoryMock = CreateIAccountRepositoryMock();
            _userRepositoryMock = CreateIUserRepositoryMock();
            _menuRepositoryMock = CreateIMenuRepositoryMock();
            _accountBusiness = new AccountBusiness(_accountRepositoryMock.Object, _userRepositoryMock.Object, _menuRepositoryMock.Object);
            _sessionMock = CreateISessionMock(ProfileCodeResources.ADMINISTRATOR);

            _signInFormVM = new SignInFormViewModel
            {
                Login = "Login",
                Password = "Password"
            };

            _linkAccountCreationVM = new LinkAccountCreationViewModel
            {
                ProfileCodeSelected = ProfileCodeResources.PLAYER,
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
            _accountRepositoryMock.Setup(m => m.GetByLoginAndPassword(It.IsAny<SignInFormViewModel>()))
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
            Assert.AreEqual(RouteBusinessResources.SUCCESS, result.RedirectUrl);
            Assert.AreEqual(_linkAccountCreationVM.RecipientMail, linkCreated.RecipientMail);
            Assert.AreEqual(_linkAccountCreationVM.ProfileCodeSelected, linkCreated.ProfileCode);
            Assert.AreEqual(DateTime.UtcNow.AddDays(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY).Day, linkCreated.ExpirationDate.Day);
        }

        [TestMethod]
        public void ShouldDontCreateLinkAccountCreation_WhenUserNotConnected()
        {
            _sessionMock = CreateISessionMock(null);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_NOT_CONNECTED, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldDontCreateLinkAccountCreation_WhenUserNotAuthorized()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.IsAny<string>(), It.IsAny<int>()))
                                .Returns(() => null);

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_CANNOT_PERFORM_ACTION, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.ACCOUNT_INVITE_WITH_ERROR_MESSAGE, MainBusinessResources.USER_CANNOT_PERFORM_ACTION), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldDontCreateLinkAccountCreation_WhenLinkAccountCreationVMIsNull()
        {
            _linkAccountCreationVM = null;

            APICallResult result = _accountBusiness.CreateLinkAccountCreation(_linkAccountCreationVM, _sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.TECHNICAL_ERROR, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.TECHNICAL_ERROR), result.RedirectUrl);
        }
    }
}
