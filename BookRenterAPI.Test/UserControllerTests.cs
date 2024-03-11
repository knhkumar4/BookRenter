using System;
using System.Threading.Tasks;
using BookRenterAPI.Controllers;
using BookRenterData.Entities;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookRenterAPI.Test
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Register_ValidUserRequest_ReturnsOk()
        {
            // Arrange
            var userRequest = new UserRequest
            {
                // Set valid user request properties here
            };

            var expectedCreatedUser = new User
            {
                // Set expected created user properties here
            };

            _userServiceMock.Setup(x => x.CreateUserAsync(userRequest))
                            .ReturnsAsync(expectedCreatedUser);

            // Act
            var result = await _userController.Register(userRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var createdUser = Assert.IsType<User>(okResult.Value);

            // Validate the properties of the created user if needed
            Assert.Equal(expectedCreatedUser.UserId, createdUser.UserId);
            Assert.Equal(expectedCreatedUser.Username, createdUser.Username);
            // Add more property validations as needed
        }

        [Fact]
        public async Task Register_ErrorInService_ReturnsInternalServerError()
        {
            // Arrange
            var userRequest = new UserRequest
            {
                // Set user request properties here
            };

            _userServiceMock.Setup(x => x.CreateUserAsync(userRequest))
                            .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _userController.Register(userRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error creating user: Simulated exception", statusCodeResult.Value);
        }

        // Add more test cases for different scenarios as needed
    }
}
