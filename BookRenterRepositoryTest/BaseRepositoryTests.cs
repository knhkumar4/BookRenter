using BookRenterData.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BookRenterData.Tests.Repositories
{
    public class BaseRepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var dbContextMock = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<object>>();

            dbContextMock.Setup(x => x.Set<object>()).Returns(dbSetMock.Object);

            var repository = new BaseRepository<object>(dbContextMock.Object);
            var entityToAdd = new object();

            // Act
            await repository.AddAsync(entityToAdd);

            // Assert
            dbSetMock.Verify(x => x.AddAsync(entityToAdd, default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            // Arrange
            var dbContextMock = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<object>>();

            dbContextMock.Setup(x => x.Set<object>()).Returns(dbSetMock.Object);

            var repository = new BaseRepository<object>(dbContextMock.Object);
            var entityToDelete = new object();

            // Act
            await repository.DeleteAsync(entityToDelete);

            // Assert
            dbSetMock.Verify(x => x.Remove(entityToDelete), Times.Once);
        }

        

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var dbContextMock = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<object>>();

            var testData = new List<object> { new object(), new object() };
            var testDataEnumerator = testData.GetEnumerator();
            var testAsyncEnumerator = new TestAsyncEnumerator<object>(testDataEnumerator);

            dbSetMock.As<IAsyncEnumerable<object>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(testAsyncEnumerator);

            dbContextMock.Setup(x => x.Set<object>()).Returns(dbSetMock.Object);

            var repository = new BaseRepository<object>(dbContextMock.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(testData, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntityById()
        {
            // Arrange
            var dbContextMock = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<object>>();

            var testData = new object();
            dbSetMock.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(testData);

            dbContextMock.Setup(x => x.Set<object>()).Returns(dbSetMock.Object);

            var repository = new BaseRepository<object>(dbContextMock.Object);
            var entityId = 1;

            // Act
            var result = await repository.GetByIdAsync(entityId);

            // Assert
            Assert.Equal(testData, result);
        }

       
    }
}
