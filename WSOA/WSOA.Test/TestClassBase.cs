using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Dtos;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.Result;
using WSOA.Shared.ViewModel;

namespace WSOA.Test
{
    [TestClass]
    public class TestClassBase
    {
        public Mock<ISession> CreateISessionMock(string? currentProfileCodeIntoSession, int? currentUserIdIntoSession)
        {
            Mock<ISession> mock = new Mock<ISession>();

            byte[]? profileCodeIntoBytes = !string.IsNullOrWhiteSpace(currentProfileCodeIntoSession) ? Encoding.UTF8.GetBytes(currentProfileCodeIntoSession) : null;
            mock.Setup(m => m.TryGetValue(HttpSessionResources.KEY_PROFILE_CODE, out profileCodeIntoBytes));

            byte[]? currentUserIdIntoBytes = currentUserIdIntoSession != null ? Encoding.UTF8.GetBytes(currentUserIdIntoSession.ToString()) : null;
            mock.Setup(m => m.TryGetValue(HttpSessionResources.KEY_USER_ID, out currentUserIdIntoBytes));

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

        public Mock<IAddressRepository> CreateIAddressRepositoryMock()
        {
            Mock<IAddressRepository> mock = new Mock<IAddressRepository>();
            return mock;
        }

        public Mock<IPlayerRepository> CreateIPlayerRepositoryMock()
        {
            Mock<IPlayerRepository> mock = new Mock<IPlayerRepository>();
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

        public Address CreateAddress(int id)
        {
            return new Address
            {
                Id = id,
                Content = "Adresse " + id.ToString()
            };
        }

        public List<Address> CreateAddresses(int number)
        {
            List<Address> addresses = new List<Address>();
            for (int i = 1; i <= number; i++)
            {
                addresses.Add(CreateAddress(i));
            }
            return addresses;
        }

        public MainNavSubSection CreateMainNavSubSection()
        {
            return new MainNavSubSection
            {
                Id = 1,
                Description = "Faire un truc",
                Label = "Truc",
                MainNavSectionId = 1,
                Order = 0,
                Url = "/url"
            };
        }

        public Tournament CreateTournament(int id)
        {
            return new Tournament
            {
                Id = id,
                AddressId = id,
                BuyIn = 1,
                IsInProgress = false,
                IsOver = false,
                Season = "Season " + id.ToString(),
                StartDate = DateTime.UtcNow.AddDays(1)
            };
        }

        public IEnumerable<TournamentDto> CreateTournamentDtos(int number, int nbPlayersByTournament)
        {
            List<TournamentDto> result = new List<TournamentDto>();
            for (int i = 1; i <= number; i++)
            {
                result.Add(CreateTournamentDto(i, nbPlayersByTournament));
            }
            return result;
        }

        public TournamentDto CreateTournamentDto(int id, int nbPlayersByTournament)
        {
            return new TournamentDto
            {
                Address = CreateAddress(id),
                Players = CreatePlayerDtos(nbPlayersByTournament),
                Tournament = CreateTournament(id)
            };
        }

        public Player CreatePlayer(int id, string? presenceStateCode = null)
        {
            return new Player
            {
                Id = id,
                PresenceStateCode = presenceStateCode != null ? presenceStateCode : PresenceStateResources.PRESENT_CODE
            };
        }

        public PlayerDto CreatePlayerDto(int id)
        {
            return new PlayerDto
            {
                Player = CreatePlayer(id),
                User = CreateUser(id)
            };
        }

        public IEnumerable<PlayerDto> CreatePlayerDtos(int number)
        {
            List<PlayerDto> result = new List<PlayerDto>();
            for (int i = 1; i <= number; i++)
            {
                result.Add(CreatePlayerDto(i));
            }
            return result;
        }

        public SignUpTournamentFormViewModel CreateSignUpTournamentFormViewModel(int tournamentId, string presenceStateCode)
        {
            return new SignUpTournamentFormViewModel
            {
                TournamentId = tournamentId,
                PresenceStateCode = presenceStateCode
            };
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

        public void VerifyAPICallResultSuccess(APICallResultBase result, string? expectedRedirectUrl)
        {
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(expectedRedirectUrl, result.RedirectUrl);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(null, result.ErrorMessage);
        }

        public void VerifyAPICallResultError(APICallResultBase result, string? expectedRedirectUrl, string expectedErrorMsg)
        {
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(expectedRedirectUrl, result.RedirectUrl);
            Assert.AreEqual(null, result.WarningMessage);
            Assert.AreEqual(expectedErrorMsg, result.ErrorMessage);
        }
    }
}
