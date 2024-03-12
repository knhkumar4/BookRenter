using System;
using System.Threading.Tasks;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using BookRenter.Services;
using Moq;
using Xunit;
using BookRenterRepository.Repositories.Interfaces;

namespace BookRenterDataServices.Test
{
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateUserAsync_ValidUserRequest_CreatesUser()
        {
            // Arrange
            var userRequest = new UserRequest
            {
                Username = "testuser",
                Password = "testpassword"
               
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, BCrypt.Net.BCrypt.GenerateSalt());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            unitOfWorkMock.SetupGet(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

            // Mock the CreateUserAsync method to simulate user creation
            userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .Callback<User>(user =>
                {
                    user.UserId = 1; 
                })
                .ReturnsAsync((User)null); 

            var userService = new UserService(unitOfWorkMock.Object);

            // Act
            var createdUser = await userService.CreateUserAsync(userRequest);

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal(userRequest.Username, createdUser.Username);

           
            userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);

           
            unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);

           
            Assert.True(BCrypt.Net.BCrypt.Verify(userRequest.Password, createdUser.PasswordHash));

        }


        [Fact]
        public async Task ValidateCredentialsAsync_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

            var user = new User
            {
                UserId = 1,
                Username = username,
                PasswordHash = hashedPassword
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            unitOfWorkMock.SetupGet(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

          
            userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync(user);

            var userService = new UserService(unitOfWorkMock.Object);

            // Act
            var validatedUser = await userService.ValidateCredentialsAsync(username, password);

            // Assert
            Assert.NotNull(validatedUser);
            Assert.Equal(user.UserId, validatedUser.UserId);
            Assert.Equal(user.Username, validatedUser.Username);
            Assert.Equal(user.PasswordHash, validatedUser.PasswordHash);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_InvalidUsername_ReturnsNull()
        {
            // Arrange
            var username = "nonexistentuser";
            var password = "testpassword";

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            unitOfWorkMock.SetupGet(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

          
            userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync((User)null);

            var userService = new UserService(unitOfWorkMock.Object);

            // Act
            var validatedUser = await userService.ValidateCredentialsAsync(username, password);

            // Assert
            Assert.Null(validatedUser);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var username = "testuser";
            var password = "incorrectpassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("testpassword", BCrypt.Net.BCrypt.GenerateSalt());

            var user = new User
            {
                UserId = 1,
                Username = username,
                PasswordHash = hashedPassword
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            unitOfWorkMock.SetupGet(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

           
            userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync(user);

            var userService = new UserService(unitOfWorkMock.Object);

            // Act
            var validatedUser = await userService.ValidateCredentialsAsync(username, password);

            // Assert
            Assert.Null(validatedUser);
        }
    }
}
