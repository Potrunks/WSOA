using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data;
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
        private static DbContextOptions<WSOADbContext> _options = new DbContextOptionsBuilder<WSOADbContext>()
                                                                     .UseInMemoryDatabase(databaseName: "WSOAUnitTest")
                                                                     .Options;

        public WSOADbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            _dbContext = new WSOADbContext(_options);
            _dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }

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

            mock.Setup(m => m.ExistsBusinessActionByProfileCode(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            mock.Setup(m => m.GetAllUsers(null))
                .Returns(CreateUsers(1));

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

            mock.Setup(m => m.ExistsTournamentByTournamentIdIsOverAndIsInProgress(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(true);

            mock.Setup(m => m.ExistsTournamentByIsInProgress(It.IsAny<bool>()))
                .Returns(false);

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

            mock.Setup(m => m.GetPlayersByTournamentIdAndPresenceStateCode(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(CreatePlayerDtos(1));

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
                Email = $"test{id}@test.com",
                FirstName = $"firstName{id}",
                LastName = $"lastName{id}",
                ProfileCode = ProfileResources.PLAYER_CODE
            };
        }

        public User SaveUser()
        {
            User usr = CreateUser(0);
            _dbContext.Add(usr);
            _dbContext.SaveChanges();
            return usr;
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

        public List<User> SaveUsers(int number)
        {
            List<User> users = new List<User>();
            for (int i = 1; i <= number; i++)
            {
                User user = CreateUser(i);
                user.Id = 0;
                users.Add(user);
            }
            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();
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

        public MainNavSubSection CreateMainNavSubSection(int id = 0, string url = "/url")
        {
            return new MainNavSubSection
            {
                Id = id,
                Description = "Faire un truc",
                Label = "Truc",
                MainNavSectionId = 1,
                Order = 0,
                Url = url
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

        public Tournament SaveTournament(int id)
        {
            Tournament tournament = CreateTournament(id);
            tournament.Id = 0;
            _dbContext.Tournaments.Add(tournament);
            _dbContext.SaveChanges();
            return tournament;
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

        public Player CreatePlayer(int id, int tournamentId = 0, int usrId = 0, string? presenceStateCode = null)
        {
            return new Player
            {
                Id = id,
                UserId = usrId,
                PlayedTournamentId = tournamentId,
                PresenceStateCode = presenceStateCode != null ? presenceStateCode : PresenceStateResources.PRESENT_CODE
            };
        }

        public Player SavePlayer(int tournamentId, int usrId, string presenceStateCode)
        {
            Player player = CreatePlayer(0, tournamentId, usrId, presenceStateCode);
            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
            return player;
        }

        public List<Player> SavePlayers(IEnumerable<int> usrIds, int tournamentId, string presenceStateCode)
        {
            List<Player> players = new List<Player>();
            foreach (int usrId in usrIds)
            {
                Player player = new Player();
                player.UserId = usrId;
                player.PlayedTournamentId = tournamentId;
                player.PresenceStateCode = presenceStateCode;
                players.Add(player);
            }
            _dbContext.Players.AddRange(players);
            _dbContext.SaveChanges();
            return players;
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

        public TournamentPreparedDto CreateTournamentPreparedDto(int tournamentId, IEnumerable<int> selectedUsrIds)
        {
            return new TournamentPreparedDto
            {
                TournamentId = tournamentId,
                SelectedUserIds = selectedUsrIds
            };
        }

        public void SaveBusinessAction(string profileCode, string businessActionCode)
        {
            BusinessAction businessAction = new BusinessAction
            {
                Code = businessActionCode,
                Label = "businessActionForTest"
            };

            BusinessActionByProfileCode businessActionByProfileCode = new BusinessActionByProfileCode
            {
                ProfileCode = profileCode,
                BusinessActionCode = businessActionCode
            };

            _dbContext.BusinessActions.Add(businessAction);
            _dbContext.BusinessActionsByProfileCode.Add(businessActionByProfileCode);
            _dbContext.SaveChanges();
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
