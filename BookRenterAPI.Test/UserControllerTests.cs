using BookRenterAPI.Controllers;
using BookRenterData.Entities;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var userRequest = new UserRequest { Username = "testUser", Password = "testPassword", Role = "user" };
            var expectedUser = new User { UserId = 1, Username = "testUser", PasswordHash = "hashedPassword", Role = "user" };

            _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<UserRequest>()))
                           .ReturnsAsync(expectedUser);

            // Act
            var actionResult = await _userController.Register(userRequest);

            // Assert
            Assert.NotNull(actionResult);

            // Ensure the action result is an OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);

            // Convert to OkObjectResult for further assertions
            var result = (OkObjectResult)actionResult;

            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.IsType<User>(result.Value);

            // Additional assertion to check the returned user's properties
            var returnedUser = (User)result.Value;
            Assert.Equal(expectedUser.UserId, returnedUser.UserId);
            Assert.Equal(expectedUser.Username, returnedUser.Username);
            Assert.Equal(expectedUser.PasswordHash, returnedUser.PasswordHash);
            Assert.Equal(expectedUser.Role, returnedUser.Role);
        }

        [Fact]
        public async Task Register_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var userRequest = new UserRequest { Username = null, Password = null, Role = null };

            // Act
            var result = await _userController.Register(userRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Username is required.", result.Value.ToString());
        }

        [Fact]
        public async Task Register_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var userRequest = new UserRequest { Username = "testUser", Password = "testPassword", Role = "user" };
            _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<UserRequest>())).ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _userController.Register(userRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal("Error creating user: Simulated exception", result.Value.ToString());
        }

    }
}
