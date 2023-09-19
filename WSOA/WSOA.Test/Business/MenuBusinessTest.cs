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

namespace WSOA.Test.Business
{
    [TestClass]
    public class MenuBusinessTest : TestClassBase
    {
        private IMenuBusiness _menuBusiness;
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<ISession> _sessionMock;

        [TestInitialize]
        public void Init()
        {
            _menuRepositoryMock = CreateIMenuRepositoryMock();
            _sessionMock = CreateISessionMock(ProfileResources.ADMINISTRATOR_CODE, null);

            _menuBusiness = new MenuBusiness(_menuRepositoryMock.Object);
        }

        [TestMethod]
        public void ShouldLoadAppropriateMainMenu_WhenUserNeedAccessIt()
        {
            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual(null, result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldDontLoadMainMenu_WhenUserSessionProfileCodeIsNull()
        {
            _sessionMock = CreateISessionMock(null, null);

            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.USER_NOT_CONNECTED, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.USER_NOT_CONNECTED), result.RedirectUrl);
        }

        [TestMethod]
        public void ShouldDontLoadMainMenu_WhenNoMainNavSectionInDB()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionsInSectionByProfileCode(It.IsAny<string>()))
                .Returns(() => null);

            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            LoadMainMenuTestFail(result);
        }

        [TestMethod]
        public void ShouldDontLoadMainMenu_WhenNoMainNavSubSectionInDB()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionsInSectionByProfileCode(It.IsAny<string>()))
                .Returns(new Dictionary<MainNavSection, List<MainNavSubSection>>
                {
                    { new MainNavSection(), null }
                });

            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            LoadMainMenuTestFail(result);
        }

        private void LoadMainMenuTestFail(MainNavMenuResult result)
        {
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.TECHNICAL_ERROR, result.ErrorMessage);
            Assert.AreEqual(string.Format(RouteBusinessResources.SIGN_IN_WITH_ERROR_MESSAGE, MainBusinessResources.TECHNICAL_ERROR), result.RedirectUrl);
        }
    }
}
