using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Test
{
    public class TestClassBase
    {
        public Mock<ISession> CreateISessionMock(string currentProfileCodeIntoSession)
        {
            Mock<ISession> mock = new Mock<ISession>();

            byte[] profileCodeIntoBytes = !string.IsNullOrWhiteSpace(currentProfileCodeIntoSession) ? Encoding.UTF8.GetBytes(currentProfileCodeIntoSession) : null;
            mock.Setup(m => m.TryGetValue(It.IsAny<string>(), out profileCodeIntoBytes))
                .Returns(true);

            return mock;
        }

        public Mock<IAccountRepository> CreateIAccountRepositoryMock()
        {
            Mock<IAccountRepository> mock = new Mock<IAccountRepository>();

            mock.Setup(m => m.GetByLoginAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Account { Id = 1, Login = "Login", Password = "Password" });

            mock.Setup(m => m.SaveLinkAccountCreation(It.IsAny<LinkAccountCreation>()))
                .Returns<LinkAccountCreation>(linkCreated => linkCreated);

            mock.Setup(m => m.GetLinkAccountCreationByMail(It.IsAny<string>()))
                .Returns(() => null);

            return mock;
        }

        public Mock<IUserRepository> CreateIUserRepositoryMock()
        {
            Mock<IUserRepository> mock = new Mock<IUserRepository>();

            mock.Setup(m => m.GetUserByAccountId(It.IsAny<int>()))
                .Returns(new User { AccountId = 1, FirstName = "FirstName", Id = 1, LastName = "LastName", ProfileCode = ProfileResources.ADMINISTRATOR_CODE });

            mock.Setup(m => m.GetAllProfiles())
                .Returns(new List<Profile>
                {
                    new Profile
                    {
                        Code = ProfileResources.ADMINISTRATOR_CODE,
                        Name = ProfileResources.ADMINISTRATOR_NAME
                    }
                });

            mock.Setup(m => m.ExistsUserByMail(It.IsAny<string>()))
                .Returns(false);

            return mock;
        }

        public Mock<IMenuRepository> CreateIMenuRepositoryMock()
        {
            Mock<IMenuRepository> mock = new Mock<IMenuRepository>();

            mock.Setup(m => m.GetMainNavSubSectionsInSectionByProfileCode(It.IsAny<string>()))
                .Returns(new Dictionary<MainNavSection, List<MainNavSubSection>>
                {
                    { new MainNavSection(), new List<MainNavSubSection> { new MainNavSubSection() } }
                });

            mock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new MainNavSubSection());

            return mock;
        }

        public Mock<ITransactionManager> CreateITransactionManagerMock()
        {
            Mock<ITransactionManager> mock = new Mock<ITransactionManager>();

            mock.Setup(m => m.BeginTransaction());

            mock.Setup(m => m.RollbackTransaction());

            mock.Setup(m => m.CommitTransaction());

            return mock;
        }

        public Mock<IMailService> CreateIMailServiceMock()
        {
            Mock<IMailService> mock = new Mock<IMailService>();

            mock.Setup(m => m.SendMailAccountCreationLink(It.IsAny<string>(), It.IsAny<string>()));

            return mock;
        }

        public Mock<ITournamentRepository> CreateITournamentRepositoryMock()
        {
            Mock<ITournamentRepository> mock = new Mock<ITournamentRepository>();

            return mock;
        }

        public LinkAccountCreation CreateLinkAccountCreation()
        {
            return new LinkAccountCreation
            {
                ExpirationDate = DateTime.UtcNow.AddDays(AccountBusinessResources.LINK_ACCOUNT_CREATION_EXPIRATION_DAY_DELAY),
                Id = 1,
                ProfileCode = ProfileResources.ADMINISTRATOR_CODE,
                RecipientMail = "test@test.test"
            };
        }

        public AccountCreationFormViewModel CreateAccountCreationFormVM()
        {
            return new AccountCreationFormViewModel
            {
                Email = "test@test.test",
                FirstName = "Prénom",
                LastName = "Nom",
                Login = "Login",
                Password = "Password",
                PasswordConfirmation = "Password"
            };
        }

        public TournamentCreationFormViewModel CreateTournamentCreationFormVM()
        {
            return new TournamentCreationFormViewModel
            {
                AddressId = 1,
                BaseUri = "www.youporn.com",
                BuyIn = 2000,
                Season = "2560",
                StartDate = DateTime.UtcNow.AddDays(7),
                SubSectionId = 1
            };
        }

        public User CreateUser(int id)
        {
            return new User
            {
                Id = id,
                AccountId = id,
                Email = "test@test.test",
                FirstName = "Alexis",
                LastName = "ARRIAL",
                ProfileCode = ProfileResources.PLAYER_CODE
            };
        }

        public List<User> CreateUsers(int number)
        {
            List<User> users = new List<User>();
            for (int i = 1; i <= number; i++)
            {
                users.Add(CreateUser(i));
            }
            return users;
        }

        public void VerifyTransactionManagerCommit(Mock<ITransactionManager> transactionManagerMock)
        {
            transactionManagerMock.Verify(m => m.BeginTransaction(), Times.Once);
            transactionManagerMock.Verify(m => m.CommitTransaction(), Times.Once);
            transactionManagerMock.Verify(m => m.RollbackTransaction(), Times.Never);
        }

        public void VerifyTransactionManagerRollback(Mock<ITransactionManager> transactionManagerMock)
        {
            transactionManagerMock.Verify(m => m.BeginTransaction(), Times.Once);
            transactionManagerMock.Verify(m => m.CommitTransaction(), Times.Never);
            transactionManagerMock.Verify(m => m.RollbackTransaction(), Times.Once);
        }
    }
}
