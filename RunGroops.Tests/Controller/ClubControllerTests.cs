using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroops.Controllers;
using RunGroops.Interfaces;
using RunGroops.Models;

namespace RunGroops.Tests.Controller {
    public class ClubControllerTests {
        private IClubRepository _clubRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;
        private ClubController _clubController;

        public ClubControllerTests() {
            // Dependencies
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();

            // SUT - System Under Test = Globaly available
            _clubController = new ClubController(_clubRepository, _photoService, _httpContextAccessor);
        }

        [Fact]
        public void ClubController_Index_ReturnsSuccess() {
            // Arrange - what i need to bring?
            var clubs = A.Fake<IEnumerable<Club>>();  // mock the clubs
            A.CallTo(() => _clubRepository.GetAll()).Returns(clubs); // mock the command (function)

            // Act
            var result = _clubController.Index();

            // Assert - Object check actions
            // Test the actions which gonna be objects and test the View model what is the returning
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ClubController_Detail_ReturnsSuccess() {
            // Arrange - what i need to bring?
            var id = 1;
            var club = A.Fake<Club>();  // mock the clubs
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club); // mock the command (function)

            // Act
            var result = _clubController.Detail(id);

            // Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
