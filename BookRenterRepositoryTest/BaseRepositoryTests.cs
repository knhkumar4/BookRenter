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

        //[Fact]
        //public async Task DeleteManyAsync_ShouldDeleteEntitiesBasedOnFilter()
        //{
        //    // Arrange
        //    var dbContextMock = new Mock<DbContext>();
        //    var dbSetMock = new Mock<DbSet<object>>();

        //    dbContextMock.Setup(x => x.Set<object>()).Returns(dbSetMock.Object);

        //    var repository = new BaseRepository<object>(dbContextMock.Object);
        //    var filter = It.IsAny<Expression<Func<object, bool>>>();

        //    // Act
        //    await repository.DeleteManyAsync(filter);

        //    // Assert
        //    dbSetMock.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<object>>()), Times.Once);
        //}

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

        //[Fact]
        //public async Task UpdateAsync_ShouldUpdateEntity()
        //{
        //    // Arrange
        //    var dbContextMock = new Mock<DbContext>();
        //    var dbSetMock = new Mock<DbSet<object>>();

        //    var testData = new object();

        //    dbContextMock.Setup(x => x.Entry(testData)).Returns(Mock.Of<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<object>>());
        //    dbContextMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.FromResult(0));

        //    dbContextMock.Setup(x => x.Set<object>()).Returns(dbSetMock.Object);

        //    var repository = new BaseRepository<object>(dbContextMock.Object);

        //    // Act
        //    var result = await repository.UpdateAsync(testData);

        //    // Assert
        //    Assert.Equal(testData, result);
        //}

        //[Fact]
        //public async Task GetManyAsync_ShouldReturnEntitiesBasedOnCriteria()
        //{
        //    // Arrange
        //    var dbContextMock = new Mock<DbContext>();
        //    var dbSetMock = new Mock<DbSet<object>>();

        //    var testData = new List<object> { new object(), new object() };
        //    var testDataEnumerator = testData.GetEnumerator();
        //    var testAsyncEnumerator = new TestAsyncEnumerator<object>(testDataEnumerator);

        //    dbSetMock.As<IAsyncEnumerable<object>>()
        //        .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
        //        .Returns(testAsyncEnumerator);

        //    dbContextMock.Setup(x => x.Set<object>()).Returns(dbSetMock.Object);

        //    var repository = new BaseRepository<object>(dbContextMock.Object);
        //    Expression<Func<object, bool>> filter = x => true;
        //    Func<IQueryable<object>, IOrderedQueryable<object>> orderBy = null;
        //    int? top = null;
        //    int? skip = null;
        //    string[] includeProperties = null;

        //    // Act
        //    var result = await repository.GetManyAsync(filter, orderBy, top, skip, includeProperties);

        //    // Assert
        //    Assert.Equal(testData, result);
        //}
    }
}
