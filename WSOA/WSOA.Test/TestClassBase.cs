﻿using Microsoft.AspNetCore.Http;
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

            return mock;
        }

        public Mock<IMenuRepository> CreateIMenuRepositoryMock()
        {
            Mock<IMenuRepository> mock = new Mock<IMenuRepository>();

            mock.Setup(m => m.GetMainNavSubSectionsBySectionAndProfileCode(It.IsAny<string>()))
                .Returns(new Dictionary<MainNavSection, List<MainNavSubSection>>
                {
                    { new MainNavSection(), new List<MainNavSubSection> { new MainNavSubSection() } }
                });

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
