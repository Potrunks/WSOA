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

        public Mock<IEliminationRepository> CreateIEliminationRepositoryMock()
        {
            return new Mock<IEliminationRepository>();
        }

        public Mock<IBonusTournamentEarnedRepository> CreateIBonusTournamentEarnedRepository()
        {
            return new Mock<IBonusTournamentEarnedRepository>();
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

        public User CreateUser(int usrId, int accountId)
        {
            return new User
            {
                Id = usrId,
                AccountId = accountId,
                Email = $"test{usrId}@test.com",
                FirstName = $"firstName{usrId}",
                LastName = $"lastName{usrId}",
                ProfileCode = ProfileResources.PLAYER_CODE
            };
        }

        public User SaveUser(int accountId)
        {
            User usr = CreateUser(0, accountId);
            _dbContext.Add(usr);
            _dbContext.SaveChanges();
            return usr;
        }

        public User SaveUser(string login, string pwd)
        {
            Account account = SaveAccount(login, pwd);
            User usr = SaveUser(account.Id);
            return usr;
        }

        public User SaveUser(string firstName, string lastName, string login, string pwd, string profileCode)
        {
            Account account = SaveAccount(login, pwd);
            User usr = new User
            {
                AccountId = account.Id,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName}.{lastName}@email.com",
                ProfileCode = profileCode
            };
            _dbContext.Users.Add(usr);
            _dbContext.SaveChanges();
            return usr;
        }

        public List<User> CreateUsers(int number)
        {
            List<User> users = new List<User>();
            for (int i = 1; i <= number; i++)
            {
                users.Add(CreateUser(i, i));
            }
            return users;
        }

        public List<User> SaveUsers(int number)
        {
            List<User> users = new List<User>();
            for (int i = 1; i <= number; i++)
            {
                User user = CreateUser(i, i);
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

        public MainNavSubSection SaveMainNavSubSection(string profileCode)
        {
            MainNavSubSection mainNavSubSection = new MainNavSubSection
            {
                Description = "Test",
                Label = "Test",
                MainNavSectionId = 0,
                Order = 0,
                Url = "Test"
            };

            _dbContext.MainNavSubSections.Add(mainNavSubSection);
            _dbContext.SaveChanges();

            MainNavSubSectionByProfileCode mainNavSubSectionByProfileCode = new MainNavSubSectionByProfileCode
            {
                MainNavSubSectionId = mainNavSubSection.Id,
                ProfileCode = profileCode
            };

            _dbContext.MainNavSubSectionsByProfileCode.Add(mainNavSubSectionByProfileCode);
            _dbContext.SaveChanges();

            return mainNavSubSection;
        }

        public Tournament CreateTournament(int id, int addressId, bool isInProgress = false, string season = "Season Test", DateTime? startDate = null, bool isOver = false)
        {
            return new Tournament
            {
                Id = id,
                AddressId = addressId,
                BuyIn = 10,
                IsInProgress = isInProgress,
                IsOver = isOver,
                Season = season,
                StartDate = startDate == null ? DateTime.UtcNow.AddDays(1) : startDate.Value
            };
        }

        public Address SaveAddress()
        {
            Address address = new Address
            {
                Content = "4 Privet Drive"
            };
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();
            return address;
        }

        public Tournament SaveTournament(bool isInProgress = false, string season = "Season Test", DateTime? startDate = null, bool isOver = false)
        {
            Address address = SaveAddress();
            Tournament tournament = CreateTournament(0, address.Id, isInProgress: isInProgress, season: season, startDate: startDate, isOver: isOver);
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
                Tournament = CreateTournament(id, id)
            };
        }

        public Player CreatePlayer(int id, int tournamentId = 0, int usrId = 0, string? presenceStateCode = null, int? positionInTournament = null, int? totalPoints = null, int? totalRebuy = null, bool? wasAddon = null, int? totalAddon = null, bool? wasFinalTable = null, int? totalWinningsAmount = null)
        {
            return new Player
            {
                Id = id,
                UserId = usrId,
                PlayedTournamentId = tournamentId,
                PresenceStateCode = presenceStateCode != null ? presenceStateCode : PresenceStateResources.PRESENT_CODE,
                CurrentTournamentPosition = positionInTournament,
                TotalWinningsPoint = totalPoints,
                TotalReBuy = totalRebuy,
                WasAddOn = wasAddon,
                WasFinalTable = wasFinalTable,
                TotalAddOn = totalAddon,
                TotalWinningsAmount = totalWinningsAmount
            };
        }

        public Player SavePlayer(int tournamentId, int usrId, string presenceStateCode, int? positionInTournament = null, int? totalPoints = null, int? totalRebuy = null, bool? wasAddon = null, int? totalAddon = null, bool? wasFinalTable = null, int? totalWinningsAmount = null)
        {
            Player player = CreatePlayer(0, tournamentId, usrId, presenceStateCode, positionInTournament, totalPoints, totalRebuy, wasAddon, totalAddon, wasFinalTable, totalWinningsAmount);
            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
            return player;
        }

        public Player SavePlayer(string firstName, string lastName, string profileCode, int tournamentId, string presenceStateCode, int? positionInTournament = null, int? totalPoints = null, int? totalRebuy = null, bool? wasAddon = null, int? totalAddon = null, bool? wasFinalTable = null, int? totalWinningsAmount = null)
        {
            User user = SaveUser(firstName, lastName, $"{lastName}Login", $"{lastName}Pwd", profileCode);
            return SavePlayer(tournamentId, user.Id, presenceStateCode, positionInTournament, totalPoints, totalRebuy, wasAddon, totalAddon, wasFinalTable, totalWinningsAmount);
        }

        public List<Player> SavePlayers(IEnumerable<int> usrIds, int tournamentId, string presenceStateCode, bool tournamentIsOver = false)
        {
            List<Player> players = new List<Player>();
            int tournamentPositionToUse = 1;
            int pointsToUse = 1000;

            foreach (int usrId in usrIds)
            {
                Player player = new Player();
                player.UserId = usrId;
                player.PlayedTournamentId = tournamentId;
                player.PresenceStateCode = presenceStateCode;
                if (tournamentIsOver)
                {
                    player.CurrentTournamentPosition = tournamentPositionToUse;
                    player.TotalWinningsPoint = pointsToUse;
                    tournamentPositionToUse++;
                    pointsToUse -= 10;
                }
                players.Add(player);
            }

            _dbContext.Players.AddRange(players);
            _dbContext.SaveChanges();
            return players;
        }

        public List<Player> SavePlayers(int tournamentId, string presenceStateCode, int playersQuantity, bool tournamentIsOver = false)
        {
            List<Account> accounts = new List<Account>();
            for (int i = 0; i < playersQuantity; i++)
            {
                Account account = SaveAccount($"Login{i}", $"Pwd{i}");
                accounts.Add(account);
            }

            List<User> users = new List<User>();
            foreach (Account accountCreated in accounts)
            {
                User user = SaveUser(accountCreated.Id);
                users.Add(user);
            }

            return SavePlayers(users.Select(usr => usr.Id), tournamentId, presenceStateCode, tournamentIsOver: tournamentIsOver);
        }

        public Account SaveAccount(string login, string pwd)
        {
            Account account = new Account
            {
                Login = login,
                Password = pwd
            };

            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
            return account;
        }

        public PlayerDto CreatePlayerDto(int id)
        {
            return new PlayerDto
            {
                Player = CreatePlayer(id),
                User = CreateUser(id, id)
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
            BusinessAction? existingBusinessAction = _dbContext.BusinessActions.SingleOrDefault(ba => ba.Code == businessActionCode);
            if (existingBusinessAction == null)
            {
                existingBusinessAction = new BusinessAction
                {
                    Code = businessActionCode,
                    Label = "businessActionForTest"
                };
                _dbContext.BusinessActions.Add(existingBusinessAction);
            }

            BusinessActionByProfileCode? existingBusinessActionByProfileCode = _dbContext.BusinessActionsByProfileCode.SingleOrDefault(bap => bap.BusinessActionCode == businessActionCode && bap.ProfileCode == profileCode);
            if (existingBusinessActionByProfileCode == null)
            {
                existingBusinessActionByProfileCode = new BusinessActionByProfileCode
                {
                    ProfileCode = profileCode,
                    BusinessActionCode = businessActionCode
                };
                _dbContext.BusinessActionsByProfileCode.Add(existingBusinessActionByProfileCode);
            }

            _dbContext.SaveChanges();
        }

        public List<BonusTournament> SaveBonusTournaments(int number)
        {
            List<BonusTournament> bonusTournaments = new List<BonusTournament>();
            for (int i = 0; i < number; i++)
            {
                BonusTournament bonusTournament = new BonusTournament
                {
                    Code = $"Code{i}",
                    Label = "Label",
                    PointAmount = i,
                    LogoPath = $"/je/suis/range/la/{i}"
                };
                bonusTournaments.Add(bonusTournament);
            }
            _dbContext.BonusTournaments.AddRange(bonusTournaments);
            _dbContext.SaveChanges();
            return bonusTournaments;
        }

        public BonusTournamentEarned SaveBonusTournamentEarned(int playerId, BonusTournament bonusTournament, int occurence = 1)
        {
            BonusTournamentEarned bonusTournamentEarned = new BonusTournamentEarned
            {
                BonusTournamentCode = bonusTournament.Code,
                Occurrence = occurence,
                PlayerId = playerId,
                PointAmount = bonusTournament.PointAmount
            };
            _dbContext.BonusTournamentEarneds.Add(bonusTournamentEarned);
            _dbContext.SaveChanges();
            return bonusTournamentEarned;
        }

        public List<BonusTournamentEarned> SaveBonusTournamentEarneds(IEnumerable<int> playerIds, IEnumerable<BonusTournament> bonusTournamentEarneds)
        {
            List<BonusTournamentEarned> createdBonusTournamentEarneds = new List<BonusTournamentEarned>();

            foreach (int playerId in playerIds)
            {
                foreach (BonusTournament bonus in bonusTournamentEarneds)
                {
                    BonusTournamentEarned bonusEarned = new BonusTournamentEarned
                    {
                        BonusTournamentCode = bonus.Code,
                        Occurrence = 1,
                        PlayerId = playerId,
                        PointAmount = 1
                    };
                    createdBonusTournamentEarneds.Add(bonusEarned);
                }
            }

            _dbContext.BonusTournamentEarneds.AddRange(createdBonusTournamentEarneds);
            _dbContext.SaveChanges();
            return createdBonusTournamentEarneds;
        }

        public Elimination SaveElimination(int eliminatedPlayerId, int eliminatorPlayerId, bool isEliminationDefinitive, DateTime? eliminationDate = null)
        {
            Elimination elimination = new Elimination
            {
                IsDefinitive = isEliminationDefinitive,
                PlayerEliminatorId = eliminatorPlayerId,
                PlayerVictimId = eliminatedPlayerId,
                CreationDate = eliminationDate ?? DateTime.UtcNow
            };
            _dbContext.Eliminations.Add(elimination);
            _dbContext.SaveChanges();
            return elimination;
        }

        public List<Elimination> SaveEliminations(int eliminatedPlayerId, int eliminatorPlayerId, int nbElimination, bool isEliminationDefinitive)
        {
            List<Elimination> eliminations = new List<Elimination>();

            for (int i = 0; i < nbElimination; i++)
            {
                Elimination elimination = new Elimination
                {
                    CreationDate = eliminations.Any() ? eliminations.Last().CreationDate.AddMinutes(30) : DateTime.UtcNow,
                    IsDefinitive = i == nbElimination - 1 ? isEliminationDefinitive : false,
                    PlayerEliminatorId = eliminatorPlayerId,
                    PlayerVictimId = eliminatedPlayerId
                };
                eliminations.Add(elimination);
            }

            _dbContext.Eliminations.AddRange(eliminations);
            _dbContext.SaveChanges();

            return eliminations;
        }

        public BonusTournament SaveBonusTournament(string code, int pointAmount, string label = "label", string logoPath = "logoPath")
        {
            BonusTournament? bonusTournament = _dbContext.BonusTournaments.SingleOrDefault(b => b.Code == code);
            if (bonusTournament == null)
            {
                bonusTournament = new BonusTournament
                {
                    Code = code,
                    Label = label,
                    PointAmount = pointAmount,
                    LogoPath = logoPath
                };
                _dbContext.BonusTournaments.Add(bonusTournament);
                _dbContext.SaveChanges();
            }
            return bonusTournament;
        }

        public BonusTournamentEarnedEditDto CreateBonusTournamentEarnedEditDto(int earnedBonusPlayerId, BonusTournament selectedBonusTournament)
        {
            return new BonusTournamentEarnedEditDto
            {
                ConcernedPlayerId = earnedBonusPlayerId,
                ConcernedBonusTournament = selectedBonusTournament
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
