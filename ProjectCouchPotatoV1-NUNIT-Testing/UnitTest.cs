using NUnit.Framework;
using ProjectCouchPotatoV1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectCouchPotatoV1.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectCouchPotatoV1.Models;
using System.Security.Claims;

namespace ProjectCouchPotatoV1.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Discover_Returns_ViewResult()
        {
            // Arrange
            var tmdbServiceMock = new Mock<ITMDBService>();
            var controller = new HomeController(tmdbServiceMock.Object, null, null, null);

            // Act
            var result = controller.Discover();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Information_Returns_ViewResult()
        {
            // Arrange
            var tmdbServiceMock = new Mock<ITMDBService>();
            var controller = new HomeController(tmdbServiceMock.Object, null, null, null);

            // Act
            var result = controller.Information();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Error_Returns_ViewResult()
        {
            // Arrange
            var tmdbServiceMock = new Mock<ITMDBService>();
            var controller = new HomeController(tmdbServiceMock.Object, null, null, null);

            // Act
            var result = controller.error();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
