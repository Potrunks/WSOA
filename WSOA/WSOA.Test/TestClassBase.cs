using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using WSOA.Server.Business.Interface;
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

            mock.Setup(m => m.GetByLoginAndPassword(It.IsAny<SignInFormViewModel>()))
                .Returns(new Account { Id = 1, Login = "Login", Password = "Password" });

            mock.Setup(m => m.SaveLinkAccountCreation(It.IsAny<LinkAccountCreation>()))
                .Returns<LinkAccountCreation>(linkCreated => linkCreated);

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

            return mock;
        }

        public Mock<IMenuRepository> CreateIMenuRepositoryMock()
        {
            Mock<IMenuRepository> mock = new Mock<IMenuRepository>();

            mock.Setup(m => m.GetMainNavSections())
                .Returns(new List<MainNavSection>
                {
                    new MainNavSection { ClassIcon = "Section 1 Icon", Id = 1, Label = "Section 1", Name = "Section 1", Order = 0 },
                    new MainNavSection { ClassIcon = "Section 2 Icon", Id = 2, Label = "Section 2", Name = "Section 2", Order = 1 }
                });

            mock.Setup(m => m.GetMainNavSubSectionsByProfileCode(ProfileResources.ADMINISTRATOR_CODE))
                .Returns(new List<MainNavSubSection> { new MainNavSubSection { Id = 1, Label = "Sub Section Admin 1", MainNavSectionId = 1, Name = "Sub Section Admin 1", Order = 0 } });

            mock.Setup(m => m.GetMainNavSubSectionsByProfileCode(ProfileResources.ORGANIZER_CODE))
                .Returns(new List<MainNavSubSection> { new MainNavSubSection { Id = 2, Label = "Sub Section Orga 1", MainNavSectionId = 2, Name = "Sub Section Orga 1", Order = 0 } });

            mock.Setup(m => m.GetMainNavSubSectionByIdAndProfileCode(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new MainNavSubSection());

            return mock;
        }

        public Mock<ITransactionManager> CreateITransactionManager()
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

            mock.Setup(m => m.SendMailAccountCreation(It.IsAny<LinkAccountCreation>()));

            return mock;
        }
    }
}
