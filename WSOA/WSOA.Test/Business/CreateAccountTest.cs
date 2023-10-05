using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Business.Utils;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Test.Business
{
    [TestClass]
    public class CreateAccountTest : TestClassBase
    {
        private AccountCreationFormViewModel _form;
        private LinkAccountCreation _linkAccountCreation;
        private Account _accountCreated;
        private User _userCreated;

        private IAccountBusiness _accountBusiness;

        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITransactionManager> _transactionManagerMock;

        [TestInitialize]
        public void Init()
        {
            _linkAccountCreation = CreateLinkAccountCreation();
            _form = CreateAccountCreationFormVM();

            _accountRepositoryMock = CreateIAccountRepositoryMock();
            _accountRepositoryMock.Setup(m => m.GetLinkAccountCreationByMail(It.IsAny<string>()))
                                  .Returns(_linkAccountCreation);
            _accountRepositoryMock.Setup(m => m.ExistsAccountByLogin(It.IsAny<string>()))
                                  .Returns(false);
            _accountRepositoryMock.Setup(m => m.SaveAccount(It.IsAny<Account>()))
                                    .Callback<Account>((account) =>
                                    {
                                        _accountCreated = account;
                                        _accountCreated.Id = 1;
                                    });
            _accountRepositoryMock.Setup(m => m.DeleteLinkAccountCreation(It.Is<LinkAccountCreation>(link => link == _linkAccountCreation)));

            _userRepositoryMock = CreateIUserRepositoryMock();
            _userRepositoryMock.Setup(m => m.ExistsUserByMail(It.IsAny<string>()))
                               .Returns(false);
            _userRepositoryMock.Setup(m => m.SaveUser(It.IsAny<User>()))
                               .Callback<User>((user) =>
                               {
                                   _userCreated = user;
                                   _userCreated.Id = 1;
                               });

            _transactionManagerMock = CreateITransactionManagerMock();

            _accountBusiness = new AccountBusiness(_accountRepositoryMock.Object, _userRepositoryMock.Object, null, _transactionManagerMock.Object, null);
        }

        [TestMethod]
        public void ShouldCreateAccount()
        {
            APICallResultBase result = _accountBusiness.CreateAccount(_form);

            _transactionManagerMock.Verify(m => m.BeginTransaction(), Times.Once());
            _accountRepositoryMock.Verify(m => m.SaveAccount(_accountCreated), Times.Once());
            Assert.AreEqual(_accountCreated.Login, _form.Login);
            Assert.AreEqual(_accountCreated.Password, _form.Password.ToSha256());
            _userRepositoryMock.Verify(m => m.SaveUser(_userCreated), Times.Once());
            Assert.AreEqual(_userCreated.AccountId, _accountCreated.Id);
            Assert.AreEqual(_userCreated.Email, _form.Email);
            Assert.AreEqual(_userCreated.FirstName, _form.FirstName);
            Assert.AreEqual(_userCreated.LastName, _form.LastName);
            Assert.AreEqual(_userCreated.ProfileCode, _linkAccountCreation.ProfileCode);
            _accountRepositoryMock.Verify(m => m.DeleteLinkAccountCreation(_linkAccountCreation), Times.Once());
            _transactionManagerMock.Verify(m => m.CommitTransaction(), Times.Once());
            _transactionManagerMock.Verify(m => m.RollbackTransaction(), Times.Never());
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(RouteBusinessResources.SIGN_IN, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateAccount_WhenLinkAccountCreationNotExist()
        {
            _accountRepositoryMock.Setup(m => m.GetLinkAccountCreationByMail(It.IsAny<string>()))
                                  .Returns(() => null);

            APICallResultBase result = _accountBusiness.CreateAccount(_form);

            _transactionManagerMock.Verify(m => m.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(m => m.CommitTransaction(), Times.Never());
            _transactionManagerMock.Verify(m => m.RollbackTransaction(), Times.Once());
            Assert.AreEqual(AccountBusinessResources.LINK_ACCOUNT_CREATION_NOT_EXIST_OR_EXPIRED, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateAccount_WhenLinkAccountCreationExpired()
        {
            _linkAccountCreation.ExpirationDate = DateTime.UtcNow.AddDays(-1);

            APICallResultBase result = _accountBusiness.CreateAccount(_form);

            _transactionManagerMock.Verify(m => m.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(m => m.CommitTransaction(), Times.Never());
            _transactionManagerMock.Verify(m => m.RollbackTransaction(), Times.Once());
            Assert.AreEqual(AccountBusinessResources.LINK_ACCOUNT_CREATION_NOT_EXIST_OR_EXPIRED, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateAccount_WhenLoginAlreadyExists()
        {
            _accountRepositoryMock.Setup(m => m.ExistsAccountByLogin(It.IsAny<string>()))
                                  .Returns(true);

            APICallResultBase result = _accountBusiness.CreateAccount(_form);

            _transactionManagerMock.Verify(m => m.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(m => m.CommitTransaction(), Times.Never());
            _transactionManagerMock.Verify(m => m.RollbackTransaction(), Times.Once());
            Assert.AreEqual(AccountBusinessResources.LOGIN_ALREADY_EXISTS, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, AccountBusinessResources.LOGIN_ALREADY_EXISTS), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldNotCreateAccount_WhenMailAlreadyExists()
        {
            _userRepositoryMock.Setup(m => m.ExistsUserByMail(It.IsAny<string>()))
                                  .Returns(true);

            APICallResultBase result = _accountBusiness.CreateAccount(_form);

            _transactionManagerMock.Verify(m => m.BeginTransaction(), Times.Once());
            _transactionManagerMock.Verify(m => m.CommitTransaction(), Times.Never());
            _transactionManagerMock.Verify(m => m.RollbackTransaction(), Times.Once());
            Assert.AreEqual(UserBusinessResources.MAIL_ALREADY_EXISTS, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, UserBusinessResources.MAIL_ALREADY_EXISTS), result.RedirectUrl);
        }
    }
}
