using Microsoft.AspNetCore.Http;
using Moq;
using WSOA.Server.Data.Interface;
using WSOA.Shared.Entity;
using WSOA.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Test
{
    public class TestClassBase
    {
        public Mock<ISession> CreateISessionMock()
        {
            Mock<ISession> mock = new Mock<ISession>();

            return mock;
        }

        public Mock<IAccountRepository> CreateIAccountRepositoryMock()
        {
            Mock<IAccountRepository> mock = new Mock<IAccountRepository>();

            mock.Setup(m => m.GetByLoginAndPassword(It.IsAny<SignInFormViewModel>()))
                .Returns(new Account { Id = 1, Login = "Login", Password = "Password" });

            return mock;
        }

        public Mock<IUserRepository> CreateIUserRepositoryMock()
        {
            Mock<IUserRepository> mock = new Mock<IUserRepository>();

            mock.Setup(m => m.GetUserByAccountId(It.IsAny<int>()))
                .Returns(new User { AccountId = 1, FirstName = "FirstName", Id = 1, LastName = "LastName", ProfileCode = ProfileCodeResources.ADMINISTRATOR });

            return mock;
        }
    }
}
