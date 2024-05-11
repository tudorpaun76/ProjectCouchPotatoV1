using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YourProject.Controllers;
using YourProject.Services;
using YourProject.Models;
using ProjectCouchPotatoV1.Areas.Identity.Data;
using ProjectCouchPotatoV1.Controllers;
using ProjectCouchPotatoV1.Models;
using ProjectCouchPotatoV1.Search;

namespace YourProject.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void HomeController_Constructor_Sets_Dependencies()
        {
            // Arrange
            var tmdbServiceMock = new Mock<ITMDBService>();
            var dbContextMock = new Mock<MovieDbContext>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Act
            var controller = new HomeController(
                tmdbServiceMock.Object,
                dbContextMock.Object,
                userManagerMock.Object,
                httpContextAccessorMock.Object
            );

            // Assert
            Assert.AreEqual(tmdbServiceMock.Object, controller._tmdbService);
            Assert.AreEqual(dbContextMock.Object, controller._dbContext);
            Assert.AreEqual(userManagerMock.Object, controller._userManager);
            Assert.AreEqual(httpContextAccessorMock.Object, controller._httpContextAccessor);
        }
    }
}
