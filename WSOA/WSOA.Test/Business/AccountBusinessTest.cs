using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Test.Business
{
    [TestClass]
    public class AccountBusinessTest : TestClassBase
    {
        private SignInFormViewModel _signInFormVM;

        private IAccountBusiness _accountBusiness;
        private Mock<ISession> _sessionMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;

        [TestInitialize]
        public void Init()
        {
            _accountRepositoryMock = CreateIAccountRepositoryMock();
            _userRepositoryMock = CreateIUserRepositoryMock();
            _accountBusiness = new AccountBusiness(_accountRepositoryMock.Object, _userRepositoryMock.Object);
            _sessionMock = CreateISessionMock(ProfileCodeResources.ADMINISTRATOR);

            _signInFormVM = new SignInFormViewModel
            {
                Login = "Login",
                Password = "Password"
            };
        }

        [TestMethod]
        public void ShouldSuccessSignIn_WhenLoginAndPasswordIsOk()
        {
            APICallResult result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
        }

        [TestMethod]
        public void ShouldFailSignIn_WhenFormModelIsNull()
        {
            APICallResult result = _accountBusiness.SignIn(null, _sessionMock.Object);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.TECHNICAL_ERROR, result.ErrorMessage);
        }

        [TestMethod]
        public void ShouldFailSignIn_WhenLoginOrPasswordIsNullOrWhiteSpace()
        {
            _signInFormVM.Login = null;
            APICallResult result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            FailSignInLoginPwdMissingTest(result);

            _signInFormVM.Login = " ";
            result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            FailSignInLoginPwdMissingTest(result);

            _signInFormVM.Login = "Login";
            _signInFormVM.Password = null;
            result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            FailSignInLoginPwdMissingTest(result);

            _signInFormVM.Password = " ";
            result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            FailSignInLoginPwdMissingTest(result);
        }

        [TestMethod]
        public void ShouldFailSignIn_WhenAccountNotFound()
        {
            _accountRepositoryMock.Setup(m => m.GetByLoginAndPassword(It.IsAny<SignInFormViewModel>()))
                                    .Returns(() => null);
            APICallResult result = _accountBusiness.SignIn(_signInFormVM, _sessionMock.Object);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(AccountBusinessResources.ERROR_SIGN_IN, result.ErrorMessage);
        }

        private void FailSignInLoginPwdMissingTest(APICallResult result)
        {
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(AccountBusinessResources.LOGIN_PWD_MISSING, result.ErrorMessage);
        }
    }
}
