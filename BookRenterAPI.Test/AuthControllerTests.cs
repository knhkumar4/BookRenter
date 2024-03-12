using BookRenter.Controllers;
using BookRenterData.Entities;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                           .ReturnsAsync(new User { Username = "test", Role = "user" });

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

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            _userServiceMock.Setup(x => x.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync((User)null);

            var loginRequest = new UserLoginRequest { Username = "invalidUser", Password = "invalidPassword" };

            // Act
            var result = await _authController.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid username or password.", result.Value);
        }

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

        // Add more test methods as needed...

        private static string GenerateJwtToken(string username, string role, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
