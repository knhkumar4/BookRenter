using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookRenter.Controllers;
using BookRenter.Models; // Assuming UserLoginRequest is in the BookRenter.Models namespace
using BookRenterData.Entities;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace BookRenterAPI.Test
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _configurationMock = new Mock<IConfiguration>();
            _authController = new AuthController(_userServiceMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            _userServiceMock.Setup(x => x.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(new User { Username = "test" });

            _configurationMock.Setup(x => x["JwtSettings:Secret"]).Returns("ThisIsAStrongAndRobustSecretKeyForJWTTokenGeneration123!@#");

            var loginRequest = new UserLoginRequest { Username = "test", Password = "test" };

            // Act
            var result = await _authController.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.GetType().GetProperty("Token")?.GetValue(result.Value));
        }

        // Similar test methods for other scenarios...

        // Example of a test for an exception scenario
        [Fact]
        public async Task Login_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _userServiceMock.Setup(x => x.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ThrowsAsync(new Exception("Simulated exception"));

            _configurationMock.Setup(x => x["JwtSettings:Secret"]).Returns("yourSecretKey");

            var loginRequest = new UserLoginRequest { Username = "validUser", Password = "validPassword" };

            // Act
            var result = await _authController.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Error logging in: Simulated exception", result.Value);
        }
    }
}
