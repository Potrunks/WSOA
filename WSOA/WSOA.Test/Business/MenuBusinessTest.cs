using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Business.Resources;
using WSOA.Server.Data.Interface;
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
            _sessionMock = CreateISessionMock(ProfileCodeResources.ADMINISTRATOR);

            _menuBusiness = new MenuBusiness(_menuRepositoryMock.Object);
        }

        [TestMethod]
        public void ShouldLoadAppropriateMainMenu_WhenUserNeedAccessIt()
        {
            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual("Section 1 Icon", result.MainNavSectionVMs[0].ClassIcon);
            Assert.AreEqual("Sub Section Admin 1", result.MainNavSectionVMs[0].MainNavSubSectionVMs.Single().Label);
            Assert.AreEqual("Section 2 Icon", result.MainNavSectionVMs[1].ClassIcon);
            Assert.AreEqual(false, result.MainNavSectionVMs[1].MainNavSubSectionVMs.Any());

            _sessionMock = CreateISessionMock(ProfileCodeResources.ORGANIZER);

            result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(null, result.ErrorMessage);
            Assert.AreEqual("Section 1 Icon", result.MainNavSectionVMs[0].ClassIcon);
            Assert.AreEqual(false, result.MainNavSectionVMs[0].MainNavSubSectionVMs.Any());
            Assert.AreEqual("Section 2 Icon", result.MainNavSectionVMs[1].ClassIcon);
            Assert.AreEqual("Sub Section Orga 1", result.MainNavSectionVMs[1].MainNavSubSectionVMs.Single().Label);
        }

        [TestMethod]
        public void ShouldDontLoadMainMenu_WhenUserSessionProfileCodeIsNull()
        {
            _sessionMock = CreateISessionMock(null);

            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            LoadMainMenuTestFail(result);
        }

        [TestMethod]
        public void ShouldDontLoadMainMenu_WhenNoMainNavSectionInDB()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSections())
                                .Returns(() => null);

            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            LoadMainMenuTestFail(result);
        }

        [TestMethod]
        public void ShouldDontLoadMainMenu_WhenNoMainNavSubSectionInDB()
        {
            _menuRepositoryMock.Setup(m => m.GetMainNavSubSectionsByProfileCode(ProfileCodeResources.ADMINISTRATOR))
                                .Returns(() => null);

            MainNavMenuResult result = _menuBusiness.LoadMainNavMenu(_sessionMock.Object);

            LoadMainMenuTestFail(result);
        }

        private void LoadMainMenuTestFail(MainNavMenuResult result)
        {
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(MainBusinessResources.TECHNICAL_ERROR, result.ErrorMessage);
        }
    }
}
