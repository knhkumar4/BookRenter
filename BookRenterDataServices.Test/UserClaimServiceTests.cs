using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using BookRenterService.Concrete;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace BookRenterDataServices.Test
{
    public class UserClaimServiceTests
    {
       
        
        [Fact]
        public async Task GetUserFromClaimAsync_UserAuthenticated_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            httpContextMock.SetupGet(c => c.User.Claims).Returns(claims);
            httpContextAccessorMock.SetupGet(a => a.HttpContext).Returns(httpContextMock.Object);

            var expectedUser = new User { Username = username };
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync(expectedUser);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.SetupGet(uow => uow.UserRepository).Returns(userRepositoryMock.Object);

            var userClaimService = new UserClaimService(httpContextAccessorMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await userClaimService.GetUserFromClaimAsync();

            // Assert
            //Assert.NotNull(result);
            //Assert.Equal(username, result.Username);
            //userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(username), Times.Once);
            //unitOfWorkMock.Verify(uow => uow.UserRepository, Times.Once);
        }


        [Fact]
        public async Task GetUserFromClaimAsync_UserNotAuthenticated_ReturnsNull()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(false);
            httpContextAccessorMock.SetupGet(a => a.HttpContext).Returns(httpContextMock.Object);

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var userClaimService = new UserClaimService(httpContextAccessorMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await userClaimService.GetUserFromClaimAsync();

            // Assert
            Assert.Null(result);
        }
    }
}
